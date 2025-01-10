using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TPS.Application;
using TPS.Infrastructure.Data;
using TPS.WebAPI.Validation;
using TSP.Domain.Entities;

namespace TPS.WebAPI;

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
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityCore<FacultyMember>().AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddIdentityCore<Student>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        //builder.Services.AddIdentityApiEndpoints<FacultyMember>();
        //builder.Services.AddIdentityApiEndpoints<Student>();

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
        services.AddSwaggerGen();
        services.AddFluentValidationRulesToSwagger();

        return services;
    }
}