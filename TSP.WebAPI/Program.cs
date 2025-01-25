using TPS.Application.Abstractions;
using TPS.Application.Services;
using TPS.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddIdentity()
    .AddMediatR()
    .AddFluentValidation()
    .AddSwagger();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();


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
