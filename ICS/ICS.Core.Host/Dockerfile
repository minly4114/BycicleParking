FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["ICS.Core.Host/ICS.Core.Host.csproj", "ICS.Core.Host/"]

RUN dotnet restore "ICS.Core.Host/ICS.Core.Host.csproj"
COPY . .
WORKDIR "/src/ICS.Core.Host"
RUN dotnet build "ICS.Core.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ICS.Core.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ICS.Core.Host.dll"]