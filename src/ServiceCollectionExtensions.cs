using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Philiprehberger.BackgroundTaskQueue;

/// <summary>
/// Extension methods for registering background task queue services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an in-memory background task queue and its hosted processing service
    /// to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configure">An optional action to configure <see cref="BackgroundQueueOptions"/>.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddBackgroundTaskQueue(
        this IServiceCollection services,
        Action<BackgroundQueueOptions>? configure = null)
    {
        var options = new BackgroundQueueOptions();
        configure?.Invoke(options);

        services.TryAddSingleton(options);
        services.TryAddSingleton<IBackgroundTaskQueue>(sp =>
            new BackgroundTaskQueue(sp.GetRequiredService<BackgroundQueueOptions>()));
        services.AddHostedService<QueuedHostedService>();

        return services;
    }
}
