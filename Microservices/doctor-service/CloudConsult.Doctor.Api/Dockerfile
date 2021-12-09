#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["doctor-service/CloudConsult.Doctor.Api/CloudConsult.Doctor.Api.csproj", "doctor-service/CloudConsult.Doctor.Api/"]
COPY ["doctor-service/CloudConsult.Doctor.Infrastructure/CloudConsult.Doctor.Infrastructure.csproj", "doctor-service/CloudConsult.Doctor.Infrastructure/"]
COPY ["doctor-service/CloudConsult.Doctor.Domain/CloudConsult.Doctor.Domain.csproj", "doctor-service/CloudConsult.Doctor.Domain/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["doctor-service/CloudConsult.Doctor.Services.MongoDb/CloudConsult.Doctor.Services.MongoDb.csproj", "doctor-service/CloudConsult.Doctor.Services.MongoDb/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
RUN dotnet restore "doctor-service/CloudConsult.Doctor.Api/CloudConsult.Doctor.Api.csproj"
COPY . .
WORKDIR "/src/doctor-service/CloudConsult.Doctor.Api"
RUN dotnet build "CloudConsult.Doctor.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Doctor.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Doctor.Api.dll"]
