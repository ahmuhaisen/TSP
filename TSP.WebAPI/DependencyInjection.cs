using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Text;
using System.Text.Json.Serialization;
using TPS.Application;
using TPS.Application.Abstractions;
using TPS.Application.Areas.AdminArea.Societies.Commands;
using TPS.Application.Areas.Authentication;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Societies;
using TPS.Application.Areas.Shared.Students;
using TPS.Application.Services;
using TPS.Application.SignalR;
using TPS.Infrastructure.BackgroundJobs;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Data.DataGenerators;
using TPS.Infrastructure.Data.Interceptors;
using TPS.Infrastructure.Emailing;
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
        services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
        {
            var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerAction =>
                {
                    sqlServerAction.EnableRetryOnFailure(3);
                    sqlServerAction.CommandTimeout(30);
                })
            .AddInterceptors(interceptor!);

            // Only in development environment
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.EnableSensitiveDataLogging();
        });

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddScoped<ApplicationDataSeeder>();

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


            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/api/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddScoped<IAuthenticationService, AuthenticationService>();

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

        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TSP", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddApplicationServicesWithOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.Configure<EmailOptions>(configuration.GetSection("Email"));

        services.AddHttpClient<GitHubService>();
        services.AddTransient<IGitHubService, GitHubService>();
        services.Configure<GitOptions>(configuration.GetSection("GitImages"));

        services.AddScoped<IPdfService, PdfService>();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
        services.AddSignalR();

        return services;
    }

    public static IServiceCollection AddApisSharedServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentsService, StudentService>();
        services.AddScoped<ISocietiesService, SocietiesService>();
        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(config =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            config.AddJob<ProcessOutboxMessagesJob>(jobKey)
                  .AddTrigger(
                        trigger => trigger.ForJob(jobKey)
                                                   .WithSimpleSchedule(schedule =>
                                                                    schedule.WithIntervalInSeconds(10).RepeatForever())
                                    );

            config.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        return services;
    }
}