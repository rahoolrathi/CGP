FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json Directory.Build.props Directory.Packages.props CgbSteps.slnx ./
COPY src/CgbSteps.Api/CgbSteps.Api.csproj src/CgbSteps.Api/
RUN dotnet restore src/CgbSteps.Api/CgbSteps.Api.csproj
COPY src/ src/
RUN dotnet publish src/CgbSteps.Api/CgbSteps.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_gcServer=0
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "CgbSteps.Api.dll"]
