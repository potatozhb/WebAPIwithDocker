#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["WebDockerAPI/WebDockerAPI.csproj", "WebDockerAPI/"]
RUN dotnet restore "WebDockerAPI/WebDockerAPI.csproj"
COPY . .
WORKDIR "/src/WebDockerAPI"
RUN dotnet build "WebDockerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebDockerAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebDockerAPI.dll"]