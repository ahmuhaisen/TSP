using Microsoft.AspNetCore.Identity;
using TPS.Application.Abstractions;
using TPS.Application.Services;
using TSP.Domain.Entities;
using TSP.Domain.Shared.Options;
using TSP.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddFluentValidation()
    .AddMediatR()
    .AddSwagger();

// TODO: Put these in the DependencyInjection.cs
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

builder.Services.AddHttpClient<GitHubService>();
builder.Services.AddTransient<IFileManagerService, GitHubService>();
builder.Services.Configure<GitOptions>(builder.Configuration.GetSection("GitImages"));

var app = builder.Build();

// TODO: Should be removed
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    string[] roles = { "Student", "Faculty" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
