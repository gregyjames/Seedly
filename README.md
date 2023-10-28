![alt text](https://github.com/gregyjames/Seedly/blob/main/res/SEEDLY.png)

### What is seedly?

Seedly is a self hosted seed box meaning that you can deploy it and use it to download torrents remotly. It consists of two main projects: SeedlyServer and SeedlyServerApp. 

### Project overview

#### Seedly Server
The backend for our application, handles the actual downloading of torrents using MonoTorrent and is connected to the GUI via GRPC streaming endpoint that recieves a request with the magnet link of a file, and returns torrent metadata and download progress back to our interface.

#### Seedly Server App
The frontend for our application. Contatins the major GUI for our application.

### Technologies used
1. Blazor 
2. MudBlazor
3. GRPC
4. ASP.NET Core

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
All contributions via pull request are greatly encouraged and welcome. Any additional features or questions can be opened via the issues page.
