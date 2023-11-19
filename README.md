[![Main Build](https://github.com/gregyjames/Seedly/actions/workflows/main.yml/badge.svg?event=push)](https://github.com/gregyjames/Seedly/actions/workflows/main.yml)
![alt text](https://github.com/gregyjames/Seedly/blob/main/res/SEEDLY.png)

### What Is Seedly?

Seedly is a self-hosted seed box, meaning that you can deploy it and use it to download torrents remotely. It consists of two main projects: SeedlyServer and SeedlyServerApp.

### Project Overview

#### Seedly Server
The backend for our application, it handles the actual downloading of torrents using [Anacrolix's Torrent Library](https://github.com/anacrolix/torrent) and is connected to the GUI via GRPC streaming endpoint that recieves a request with the magnet link of a file, and returns torrent metadata and download progress back to our interface.

#### Seedly Server App
The frontend for our application, which contains the major GUI for the application.

### Technologies Used
1. Blazor 
2. MudBlazor
3. GRPC
4. ASP.NET Core
5. Go

### Running
```
docker compose up -d
```

### License
MIT License

Copyright (c) 2023 Greg James

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

### Contributions
This project is still in its infancy and there are many features to be added and bugs to be resolved. All contributions via pull request are greatly encouraged and welcome. Any additional features or questions can be opened via the issues page.
