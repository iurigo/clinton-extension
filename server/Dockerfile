# Build
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal as build-env

WORKDIR /app

COPY ./*.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /app/dist

# Install
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal

WORKDIR /app

# Change timezone to local time
RUN apt update && apt install -y tzdata libgdiplus

COPY --from=build-env /app/dist .

ENTRYPOINT ["dotnet", "server.dll"]