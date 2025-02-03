using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using TPS.Application;
using TPS.Application.Abstractions;
using TPS.Application.Services;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared.Options;
using TSP.WebAPI;
using TSP.WebAPI.Validation;

namespace TSP.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = ModelStateValidator.ValidateModelState;
            });

        return services;
    }

    public static IServiceCollection AddEntityFrameworkStore(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerAction =>
                {
                    sqlServerAction.EnableRetryOnFailure(3);
                    sqlServerAction.CommandTimeout(30);
                });

            // Only in development environment
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        });

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(ApplicationAssemblyReference.Assembly);
        });

        return services;
    }

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
#pragma warning disable CS0618
        services.AddFluentValidation(config =>
        {
            config.RegisterValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
        });
#pragma warning restore CS0618

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddFluentValidationRulesToSwagger();

        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddApplicationServicesWithOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        services.AddHttpClient<GitHubService>();
        services.AddTransient<IGitHubService, GitHubService>();
        services.Configure<GitOptions>(configuration.GetSection("GitImages"));

        return services;
    }
}