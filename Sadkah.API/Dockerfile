FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Sadkah.API.csproj .
RUN dotnet restore Sadkah.API.csproj

COPY . .
RUN dotnet publish Sadkah.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Tesseract NuGet package is a wrapper; the native leptonica/tesseract libs it p/invokes
# into aren't part of the .NET runtime image and must be installed separately.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libleptonica-dev libtesseract-dev curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# tessdata/eng.traineddata is gitignored (large binary), so it won't exist in a fresh
# checkout on the VPS. Fetch it at build time instead of relying on COPY finding it.
RUN mkdir -p tessdata \
    && curl -fsSL -o tessdata/eng.traineddata \
       https://github.com/tesseract-ocr/tessdata/raw/main/eng.traineddata

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Sadkah.API.dll"]
