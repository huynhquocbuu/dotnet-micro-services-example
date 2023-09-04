using Infrastructure.Middlewares;

namespace Product.API.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.EnvironmentName.Equals("Docker"))
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c
                .SwaggerEndpoint("/swagger/v1/swagger.json", $"Product API {env.EnvironmentName}"));
        }
        app.UseMiddleware<ErrorWrappingMiddleware>();
        app.UseRouting();
        // app.UseHttpsRedirection(); //for production only

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}