#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["member-service/CloudConsult.Member.Api/CloudConsult.Member.Api.csproj", "member-service/CloudConsult.Member.Api/"]
COPY ["member-service/CloudConsult.Member.Infrastructure/CloudConsult.Member.Infrastructure.csproj", "member-service/CloudConsult.Member.Infrastructure/"]
COPY ["member-service/CloudConsult.Member.Domain/CloudConsult.Member.Domain.csproj", "member-service/CloudConsult.Member.Domain/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
COPY ["member-service/CloudConsult.Member.Services.MongoDb/CloudConsult.Member.Services.MongoDb.csproj", "member-service/CloudConsult.Member.Services.MongoDb/"]
RUN dotnet restore "member-service/CloudConsult.Member.Api/CloudConsult.Member.Api.csproj"
COPY . .
WORKDIR "/src/member-service/CloudConsult.Member.Api"
RUN dotnet build "CloudConsult.Member.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Member.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Member.Api.dll"]
