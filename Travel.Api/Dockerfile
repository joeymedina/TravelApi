FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Travel.Api/Travel.Api.csproj", "Travel.Api/"]
COPY ["Travel.Application/Travel.Application.csproj", "Travel.Application/"]
COPY ["Travel.Domain/Travel.Domain.csproj", "Travel.Domain/"]
COPY ["Travel.Model/Travel.Model.csproj", "Travel.Model/"]
COPY ["Travel.Infrastructure/Travel.Infrastructure.csproj", "Travel.Infrastructure/"]
RUN dotnet restore "Travel.Api/Travel.Api.csproj"
COPY . .
WORKDIR "/src/Travel.Api"
RUN dotnet build "./Travel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Travel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Travel.Api.dll"]
