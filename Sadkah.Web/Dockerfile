FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


# Restoring from just the .csproj (before the rest of the source is copied) makes the SDK
# resolve/skip the Microsoft.AspNetCore.App.Internal.Assets package that ships blazor.web.js,
# so publish --no-restore later can't get it and the framework script 404s at runtime.
# Copy the full source before restoring so that package resolves correctly.
COPY . .
RUN dotnet publish Sadkah.Web.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Sadkah.Web.dll"]
