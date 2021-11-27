#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["identity-service/CloudConsult.Identity.Api/CloudConsult.Identity.Api.csproj", "identity-service/CloudConsult.Identity.Api/"]
COPY ["identity-service/CloudConsult.Identity.Domain/CloudConsult.Identity.Domain.csproj", "identity-service/CloudConsult.Identity.Domain/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["identity-service/CloudConsult.Identity.Infrastructure/CloudConsult.Identity.Infrastructure.csproj", "identity-service/CloudConsult.Identity.Infrastructure/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
COPY ["identity-service/CloudConsult.Identity.Services.SqlServer/CloudConsult.Identity.Services.SqlServer.csproj", "identity-service/CloudConsult.Identity.Services.SqlServer/"]
RUN dotnet restore "identity-service/CloudConsult.Identity.Api/CloudConsult.Identity.Api.csproj"
COPY . .
WORKDIR "/src/identity-service/CloudConsult.Identity.Api"
RUN dotnet build "CloudConsult.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Identity.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Identity.Api.dll"]
