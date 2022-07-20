#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 41067

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EAuction.Seller/EAuction.Seller.csproj", "EAuction.Seller/"]
COPY ["EAuction.Processor/EAuction.Processor.csproj", "EAuction.Processor/"]
COPY ["EAuction.Modal/EAuction.Models.csproj", "EAuction.Modal/"]
COPY ["EAuction.Data/EAuction.Data.csproj", "EAuction.Data/"]
RUN dotnet restore "EAuction.Seller/EAuction.Seller.csproj"
COPY . .
WORKDIR "/src/EAuction.Seller"
RUN dotnet build "EAuction.Seller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAuction.Seller.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAuction.Seller.dll"]