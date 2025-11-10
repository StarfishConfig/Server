using Nerosoft.Euonia.Hosting;
using Nerosoft.Starfish.Webapi;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureSerilog();

// Add services to the container.
builder.Services.AddModularityApplication<HostServiceModule>(builder.Configuration);

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>().LogInformation("Application started");
    // Custom logic to execute when the application has started
});

app.Lifetime.ApplicationStopped.Register(() =>
{
    app.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>().LogInformation("Application stopped");
});

app.InitializeApplication();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //endpoints.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("health");

app.Run();