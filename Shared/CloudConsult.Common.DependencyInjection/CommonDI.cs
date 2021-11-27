using CloudConsult.Common.Builders;
using CloudConsult.Common.Configurations;
using CloudConsult.Common.Email;
using CloudConsult.Common.Encryption;
using CloudConsult.Common.Middlewares;
using Confluent.Kafka;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace CloudConsult.Common.DependencyInjection;
public static class CommonDI
{
    public static IServiceCollection AddCommonApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
        return services;
    }

    public static IServiceCollection AddCommonMediatorConfiguration(this IServiceCollection services, params string[] projectNames)
    {
        var assemblies = projectNames.Select(Assembly.Load).ToArray();
        services.AddMediatR(assemblies);
        services.AddTransient(typeof(IApiResponseBuilder<>), typeof(ApiResponseBuilder<>));
        services.AddTransient<IApiResponseBuilder, ApiResponseBuilder>();
        return services;
    }

    public static IServiceCollection AddCommonKafkaProducer(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new KafkaConfiguration();
        configuration.Bind(nameof(KafkaConfiguration), config);
        services.AddSingleton(config);

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.BootstrapServers,
            Acks = config.Acks == "All" ? Acks.All : config.Acks == "Leader" ? Acks.Leader : Acks.None,
            MessageSendMaxRetries = config.MessageSendMaxRetries,
            MessageTimeoutMs = config.MessageTimeoutMs,
            RetryBackoffMs = config.RetryBackoffMs,
            EnableIdempotence = config.EnableIdempotence
        };

        var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        services.AddSingleton(producer);
        return services;
    }

    public static IServiceCollection AddCommonJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration();
        configuration.Bind(nameof(jwtConfiguration), jwtConfiguration);
        services.AddSingleton(jwtConfiguration);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.SecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        });
        return services;
    }

    public static IServiceCollection AddCommonValidationsFrom(this IServiceCollection services, params string[] projectNames)
    {
        var assemblies = projectNames.Select(Assembly.Load).ToArray();
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }

    public static void AddCommonEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailServiceConfiguration = new EmailServiceConfiguration();
        configuration.Bind(nameof(emailServiceConfiguration), emailServiceConfiguration);
        services.AddSingleton(emailServiceConfiguration);

        services.AddScoped<IEmailService, EmailService>();
    }

    public static IServiceCollection AddCommonHashingService(this IServiceCollection services)
    {
        services.AddScoped<IHashingService, HashingService>();
        return services;
    }

    public static IServiceCollection AddCommonMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        return services;
    }
}