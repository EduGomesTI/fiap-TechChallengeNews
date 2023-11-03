#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebAPI/News.WebAPI.csproj", "WebAPI/"]
COPY ["Application/News.Application.csproj", "Application/"]
COPY ["Domain/News.Domain.csproj", "Domain/"]
COPY ["Infrastructure/News.Infrastructure.csproj", "Infrastructure/"]
COPY ["Security.JWT/News.Security.JWT.csproj", "Security.JWT/"]
RUN dotnet restore "WebAPI/News.WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "News.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "News.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "News.WebAPI.dll"]