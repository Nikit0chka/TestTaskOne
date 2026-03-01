using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestTask.Application.Extensions;

/// <summary>
///     Методы расширения для работы с ioc-контейнером.
/// </summary>
public static class ServiceCollectionsExtensions
{
    /// <summary>
    ///     Добавляет application сервисы в ioc-контейнер.
    /// </summary>
    /// <param name="serviceCollection"> Ioc-контейнер. </param>
    /// <param name="logger"> Логер. </param>
    public static void AddApplicationServices(this IServiceCollection serviceCollection, ILogger logger)
    {
        logger.LogInformation("Добавление application сервисов...");

        serviceCollection.AddMediator(static options =>
        {
            options.Namespace = "TestTask.Application";
            options.ServiceLifetime = ServiceLifetime.Singleton;
            options.GenerateTypesAsInternal = true;
        });

        logger.LogInformation("Application сервисы добавлены");
    }
}
