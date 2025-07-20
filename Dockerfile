# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy everything and restore
COPY . ./
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o out

# Use the official .NET 8 ASP.NET runtime for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ChatApp.API.dll"]