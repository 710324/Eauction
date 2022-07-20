#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 41068

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["EAuction.Buyer/EAuction.Buyer.csproj", "EAuction.Buyer/"]
COPY ["EAuction.Processor/EAuction.Processor.csproj", "EAuction.Processor/"]
COPY ["EAuction.Modal/EAuction.Models.csproj", "EAuction.Modal/"]
COPY ["EAuction.Data/EAuction.Data.csproj", "EAuction.Data/"]
RUN dotnet restore "EAuction.Buyer/EAuction.Buyer.csproj"
COPY . .
WORKDIR "/src/EAuction.Buyer"
RUN dotnet build "EAuction.Buyer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EAuction.Buyer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAuction.Buyer.dll"]