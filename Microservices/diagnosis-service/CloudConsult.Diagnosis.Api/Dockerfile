#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["diagnosis-service/CloudConsult.Diagnosis.Api/CloudConsult.Diagnosis.Api.csproj", "diagnosis-service/CloudConsult.Diagnosis.Api/"]
COPY ["diagnosis-service/CloudConsult.Diagnosis.Services.MongoDb/CloudConsult.Diagnosis.Services.MongoDb.csproj", "diagnosis-service/CloudConsult.Diagnosis.Services.MongoDb/"]
COPY ["diagnosis-service/CloudConsult.Diagnosis.Domain/CloudConsult.Diagnosis.Domain.csproj", "diagnosis-service/CloudConsult.Diagnosis.Domain/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["diagnosis-service/CloudConsult.Diagnosis.Infrastructure/CloudConsult.Diagnosis.Infrastructure.csproj", "diagnosis-service/CloudConsult.Diagnosis.Infrastructure/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
RUN dotnet restore "diagnosis-service/CloudConsult.Diagnosis.Api/CloudConsult.Diagnosis.Api.csproj"
COPY . .
WORKDIR "/src/diagnosis-service/CloudConsult.Diagnosis.Api"
RUN dotnet build "CloudConsult.Diagnosis.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Diagnosis.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Diagnosis.Api.dll"]
