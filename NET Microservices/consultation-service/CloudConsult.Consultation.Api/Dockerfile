#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["consultation-service/CloudConsult.Consultation.Api/CloudConsult.Consultation.Api.csproj", "consultation-service/CloudConsult.Consultation.Api/"]
COPY ["consultation-service/CloudConsult.Consultation.Services.SqlServer/CloudConsult.Consultation.Services.SqlServer.csproj", "consultation-service/CloudConsult.Consultation.Services.SqlServer/"]
COPY ["consultation-service/CloudConsult.Consultation.Domain/CloudConsult.Consultation.Domain.csproj", "consultation-service/CloudConsult.Consultation.Domain/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["consultation-service/CloudConsult.Consultation.Infrastructure/CloudConsult.Consultation.Infrastructure.csproj", "consultation-service/CloudConsult.Consultation.Infrastructure/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
RUN dotnet restore "consultation-service/CloudConsult.Consultation.Api/CloudConsult.Consultation.Api.csproj"
COPY . .
WORKDIR "/src/consultation-service/CloudConsult.Consultation.Api"
RUN dotnet build "CloudConsult.Consultation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Consultation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Consultation.Api.dll"]
