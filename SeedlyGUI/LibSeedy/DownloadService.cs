﻿using Grpc.Core;
using MonoTorrent;
using MonoTorrent.Client;
using Seedly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace LibSeedy
{
    public class DownloadService : Seedly.Seedly.SeedlyBase
    {
        private EngineSettingsBuilder _settingBuilder;
        private ArrayPool<byte> _pool;

        public DownloadService() {
            _settingBuilder = new EngineSettingsBuilder()
            {
                AllowPortForwarding = true,
                AutoSaveLoadDhtCache = true,
                AutoSaveLoadFastResume = true,
                AutoSaveLoadMagnetLinkMetadata = true,
                ListenPort = 55123,
                DhtPort = 55123,
                HttpStreamingPrefix = new Uri("http://127.0.0.1:55123/")
            };
            _pool = ArrayPool<byte>.Shared;
        }

        public override async Task<InfoResponse> GetTorrentInfo(InfoRequest request, ServerCallContext context)
        {
            var magnet = MagnetLink.TryParse(request.Url, out var magnetLink);
            if (magnet)
            {
                var info = await GetInfo(magnetLink, context.CancellationToken);
                return info;
            }
            else
            {
                return new InfoResponse
                {
                    Files = ""
                };
            }
        }

        private async Task<InfoResponse> GetInfo(MagnetLink link, CancellationToken token)
        {
            InfoResponse info = new InfoResponse();
            using (var engine = new ClientEngine(_settingBuilder.ToSettings()))
            {
                var manager = await engine.AddStreamingAsync(link, "downloads");
                await manager.StartAsync();
                await manager.WaitForMetadataAsync(token);
                info.Files = string.Join(",", manager.Files.OrderByDescending(t => t.Length).Select(x => x.Path));
                await manager.StopAsync();
            }
            return info;
        }

        public async Task DownloadAsync(MagnetLink link, string outfile, IProgress<double> progress, CancellationToken token)
        {
            using (var engine = new ClientEngine(_settingBuilder.ToSettings()))
            {
                var torrentSettings = new TorrentSettingsBuilder()
                {
                    MaximumUploadSpeed = 1,
                    UploadSlots = 1,
                    CreateContainingDirectory = false,
                };
                var manager = await engine.AddStreamingAsync(link, "downloads", torrentSettings.ToSettings());
                await manager.StartAsync();
                await manager.WaitForMetadataAsync(token);

                var largestFile = manager.Files.OrderByDescending(t => t.Length).First();
                var stream = await manager.StreamProvider.CreateStreamAsync(largestFile, false, token);
               
                using (var s = new FileStream(largestFile.Path, FileMode.OpenOrCreate))
                {
                    var buffer = _pool.Rent(8196);
                    long totalBytesRead = 0;
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0)
                    {
                        await s.WriteAsync(buffer, 0, bytesRead, token);
                        totalBytesRead += bytesRead;

                        // Calculate the progress as a percentage and report it
                        double downloadProgress = (double)totalBytesRead / largestFile.Length * 100;
                        progress.Report(downloadProgress);
                    }
                    _pool.Return(buffer, true);
                }

                await manager.StopAsync();
            }
        }
        public override async Task GetUpdateStream(DownloadRequest request, IServerStreamWriter<Seedly.Update> responseStream, ServerCallContext context)
        {
            int i = 0;
            string old = request.Url;
            while (!context.CancellationToken.IsCancellationRequested && i < 100)
            {
                var progress = new Progress<double>(async (x) =>
                {
                    lock (responseStream)
                    {
                        responseStream.WriteAsync(new Update { 
                            ProgressInt = (int)x
                        }).Wait();
                        i = (int)x;
                    }
                });

                MagnetLink.TryParse(request.Url, out var link);
                await DownloadAsync(link, request.Outfile, progress, context.CancellationToken);
            }
        }
    }
}
