using Microsoft.AspNetCore.Identity;
using TSP.Domain.Entities;
using TSP.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddApplicationServicesWithOptions(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddFluentValidation()
    .AddMediatR()
    .AddSwagger();

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
