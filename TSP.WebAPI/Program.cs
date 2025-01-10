using TPS.WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiControllers()
    .AddEntityFrameworkStore(builder.Configuration)
    .AddIdentity()
    .AddMediatR()
    .AddFluentValidation()
    .AddSwagger();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
