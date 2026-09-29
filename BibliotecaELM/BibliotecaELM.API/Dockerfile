FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BibliotecaELM.API/BibliotecaELM.API.csproj", "BibliotecaELM.API/"]
COPY ["BibliotecaELM.Application/BibliotecaELM.Application.csproj", "BibliotecaELM.Application/"]
COPY ["BibliotecaELM.Domain/BibliotecaELM.Domain.csproj", "BibliotecaELM.Domain/"]
COPY ["BibliotecaELM.Infrastructure/BibliotecaELM.Infrastructure.csproj", "BibliotecaELM.Infrastructure/"]
RUN dotnet restore "BibliotecaELM.API/BibliotecaELM.API.csproj"
COPY . .
WORKDIR "/src/BibliotecaELM.API"
RUN dotnet build "./BibliotecaELM.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BibliotecaELM.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BibliotecaELM.API.dll"]
