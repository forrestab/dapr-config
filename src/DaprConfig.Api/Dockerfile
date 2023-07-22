#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/DaprConfig.Api/DaprConfig.Api.csproj", "src/DaprConfig.Api/"]
RUN dotnet restore "src/DaprConfig.Api/DaprConfig.Api.csproj"
COPY . .
WORKDIR "/src/src/DaprConfig.Api"
RUN dotnet build "DaprConfig.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprConfig.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprConfig.Api.dll"]