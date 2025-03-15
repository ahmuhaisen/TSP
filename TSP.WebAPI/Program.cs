using TPS.Infrastructure.DataGenerators;
using TSP.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddApplicationServicesWithOptions(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddFluentValidation()
    .AddMediatR()
    .AddSwagger()
    .AddApisSharedServices();


var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDataSeeder>();
//    await seeder.Seed();
//}

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
