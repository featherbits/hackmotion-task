FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /source
COPY ./back-end ./
RUN dotnet restore
WORKDIR /source/AnalyticsService
RUN dotnet publish -c release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /var/hackmotion
COPY --from=build /dist ./
ENTRYPOINT ["dotnet", "AnalyticsService.dll"]