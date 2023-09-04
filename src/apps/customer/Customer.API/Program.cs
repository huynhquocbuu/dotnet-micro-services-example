using Common.Logging;
using Customer.API;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Services;
using Infrastructure.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);
Log.Information($"Start {builder.Environment.ApplicationName}-{builder.Environment.EnvironmentName} up");

try
{
    builder.Host.UseSerilog(SeriLogger.Configure);
    builder.Host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services
        .AddDbContext<CustomerDbContext>(options => options.UseNpgsql(connectionString))
        .AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped<ICustomerService, CustomerService>();
    
    var app = builder.Build();
    
    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}!");
    app.MapGet("/api/customers/{username}",
        async (string username, ICustomerService customerService) =>
        {
            var result = await customerService.GetCustomerByUsernameAsync(username);
            return result != null ? result : Results.NotFound();
        });
    
    
    //app.MapCustomersAPI();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Docker"))
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
            c.SwaggerEndpoint(
                "/swagger/v1/swagger.json",
                $"Swagger {builder.Environment.ApplicationName} v1"));
    }
    app.UseMiddleware<ErrorWrappingMiddleware>();
    //app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();
    //app.MapDefaultControllerRoute();

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information($"Shutdown {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}