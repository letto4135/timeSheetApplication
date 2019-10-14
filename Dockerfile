FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY timeSheetApplication/*.csproj ./app/timeSheetApplication/
WORKDIR /app/timeSheetApplication
RUN dotnet restore
COPY timeSheetApplication/. ./
RUN dotnet publish -o out /p:PublishWithAspNetCoreTargetManifest="true"
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
ENV ASPNETCORE_URLS http://+:80
WORKDIR /app
COPY --from=build /app/timeSheetApplication/out ./
ENTRYPOINT ["dotnet", "timeSheetApplication.dll"]