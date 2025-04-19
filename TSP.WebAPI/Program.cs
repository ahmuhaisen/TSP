using TPS.Application.SignalR;
using TSP.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddApplicationServicesWithOptions(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddFluentValidation()
    .AddBackgroundJobs()
    .AddMediatR()
    .AddSwagger()
    .AddApisSharedServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var allowedOrigin = "http://localhost:4200"; // your Angular dev URL

app.UseCors(policy =>
{
    policy.WithOrigins(allowedOrigin)
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials(); // this is important!
});

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("api/hubs/notifications");

app.Run();
