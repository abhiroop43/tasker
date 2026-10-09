namespace Tasker.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        ConfigureHostBuilder hostBuilder
    )
    {
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        services.AddCarter();

        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        hostBuilder.UseWolverine(opts =>
        {
            opts.UseEntityFrameworkCoreTransactions();
        });
        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.MapOpenApi();

        app.MapCarter();

        app.UseHttpsRedirection();
        return app;
    }
}
