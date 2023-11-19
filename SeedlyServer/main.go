package main

import (
	"context"
	"log"
	"net"
	pb_ref "seedlyserver/proto"
	"time"

	"github.com/anacrolix/torrent"
	"github.com/cheggaaa/pb/v3"
	"google.golang.org/grpc"
)

// Server implements the Seedly gRPC service
type server struct {
	pb_ref.UnimplementedSeedlyServer
}

// GetUpdateStream is the implementation of the GetUpdateStream RPC
func (s server) GetUpdateStream(req *pb_ref.DownloadRequest, stream pb_ref.Seedly_GetUpdateStreamServer) error {
	log.Println("New download request recieved...")
	c, _ := torrent.NewClient(nil)
	defer c.Close()
	t, _ := c.AddMagnet(req.Url)
	<-t.GotInfo()
	t.DisallowDataUpload()
	t.DownloadAll()

	// Create a new progress bar
	bar := pb.New64(int64(t.Length())).Set(pb.Bytes, true).SetRefreshRate(time.Second).Start()

	// Channel to signal that download is complete
	downloadComplete := make(chan struct{})

	go func() {
		defer close(downloadComplete)
		defer bar.Finish()

		for t.BytesCompleted() < t.Length() {
			// Update the progress bar
			bar.SetCurrent(t.BytesCompleted())
			update := &pb_ref.Update{
				ProgressInt: float32(100.0 * (float64(t.BytesCompleted()) / float64(t.Length()))),
			}
			if err := stream.Send(update); err != nil {
				log.Printf("send error %v", err)
				return
			}
			time.Sleep(time.Second)
		}

		log.Println("Download Complete.")
	}()

	// Wait for either the download to complete or the client to cancel
	select {
	case <-downloadComplete:
		// Download is complete, nothing to do here
		log.Println("Download completed successfully.")
	case <-stream.Context().Done():
		// Client canceled the download
		log.Println("Client canceled the download.")
	}

	return nil
}

func (s server) GetTorrentInfo(ctx context.Context, req *pb_ref.InfoRequest) (*pb_ref.InfoResponse, error) {
	log.Println("Torrent info request recieved...")
	c, _ := torrent.NewClient(nil)
	defer c.Close()
	t, _ := c.AddMagnet(req.Url)
	<-t.GotInfo()

	files := ""

	for _, element := range t.Files() {
		files = files + "," + element.DisplayPath()
	}

	resp := &pb_ref.InfoResponse{
		Files: files,
		Name:  t.Name(),
	}
	log.Println("Torrent info request complete.")
	return resp, nil
}
func main() {
	lis, err := net.Listen("tcp", ":50005")
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}

	// create grpc server
	s := grpc.NewServer()
	pb_ref.RegisterSeedlyServer(s, server{})
	log.Println("start server")
	// and start...
	if err := s.Serve(lis); err != nil {
		log.Fatalf("failed to serve: %v", err)
	}
}
