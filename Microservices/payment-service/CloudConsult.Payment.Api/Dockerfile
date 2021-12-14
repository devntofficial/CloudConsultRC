#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["payment-service/CloudConsult.Payment.Api/CloudConsult.Payment.Api.csproj", "payment-service/CloudConsult.Payment.Api/"]
COPY ["payment-service/CloudConsult.Payment.Infrastructure/CloudConsult.Payment.Infrastructure.csproj", "payment-service/CloudConsult.Payment.Infrastructure/"]
COPY ["../Shared/CloudConsult.Common/CloudConsult.Common.csproj", "../Shared/CloudConsult.Common/"]
COPY ["payment-service/CloudConsult.Payment.Domain/CloudConsult.Payment.Domain.csproj", "payment-service/CloudConsult.Payment.Domain/"]
COPY ["../Shared/CloudConsult.Common.DependencyInjection/CloudConsult.Common.DependencyInjection.csproj", "../Shared/CloudConsult.Common.DependencyInjection/"]
RUN dotnet restore "payment-service/CloudConsult.Payment.Api/CloudConsult.Payment.Api.csproj"
COPY . .
WORKDIR "/src/payment-service/CloudConsult.Payment.Api"
RUN dotnet build "CloudConsult.Payment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudConsult.Payment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudConsult.Payment.Api.dll"]
