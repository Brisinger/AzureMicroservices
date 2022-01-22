#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ECommerce.API.Search/ECommerce.API.Search.csproj", "ECommerce.API.Search/"]
RUN dotnet restore "ECommerce.API.Search/ECommerce.API.Search.csproj"
COPY . .
WORKDIR "/src/ECommerce.API.Search"
RUN dotnet build "ECommerce.API.Search.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerce.API.Search.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.API.Search.dll"]
