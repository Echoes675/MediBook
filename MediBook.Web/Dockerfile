ARG APP_UID=1000

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID
RUN adduser --uid $APP_UID --disabled-password --gecos "" appuser
USER appuser
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
ARG APP_UID
WORKDIR /app
COPY --from=publish /app/publish .
RUN chown -R $APP_UID:$APP_UID /app
USER appuser
ENTRYPOINT ["dotnet", "MediBook.Web.dll"]
