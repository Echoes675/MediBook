FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8100
EXPOSE 8101

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MediBook.Web/MediBook.Web.csproj", "MediBook.Web/"]
COPY ["MediBook.Data/MediBook.Data.csproj", "MediBook.Data/"]
COPY ["MediBook.Core/MediBook.Core.csproj", "MediBook.Core/"]
COPY ["MediBook.Services/MediBook.Services.csproj", "MediBook.Services/"]
RUN dotnet restore "./MediBook.Web/MediBook.Web.csproj"
COPY . .
WORKDIR "/src/MediBook.Web"
RUN dotnet build "./MediBook.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MediBook.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN chown -R app:app /app
USER app
ENV ASPNETCORE_URLS="http://+:8100;https://+:8101"
ENTRYPOINT ["dotnet", "MediBook.Web.dll"]
