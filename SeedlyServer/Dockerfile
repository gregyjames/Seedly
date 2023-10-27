FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SeedlyServer/SeedlyServer.csproj", "SeedlyServer/"]
RUN dotnet restore "SeedlyServer/SeedlyServer.csproj"
COPY . .
WORKDIR "/src/SeedlyServer"
RUN dotnet build "SeedlyServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeedlyServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeedlyServer.dll"]
