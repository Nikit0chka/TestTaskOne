using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTask.Application.Contracts;
using TestTask.Domain.CarAggregate;
using TestTask.Domain.WeightingAggregate;
using TestTask.Infrastructure.Data;
using TestTask.Infrastructure.Services;
using TestTask.Infrastructure.Services.Repositories;

namespace TestTask.Infrastructure.Extensions;

/// <summary>
///     Методы-расширения для работы с ioc-контейнером.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <param name="serviceCollection"> Ioc-контейнер. </param>
    extension(IServiceCollection serviceCollection)
    {
        /// <summary>
        ///     Зарегистрировать infrastructure сервисы в ioc-контейнер.
        /// </summary>
        /// <param name="configurationManager"> Менеджер конфигураций. </param>
        /// <param name="logger"> Логер. </param>
        public void AddInfrastructureServices(IConfiguration configurationManager,
            ILogger logger)
        {
            logger.LogInformation("Регистрация infrastructure сервисов...");

            var dbConnectionString = configurationManager.GetConnectionString("DefaultConnection");

            serviceCollection.AddDbContext<WeightingContext>(options => options.UseNpgsql(dbConnectionString));

            serviceCollection.AddRepositories();

            serviceCollection.AddTransient<IWeightingFileService, WeightingFileService>();

            logger.LogInformation("Infrastructure сервисы зарегистрированы");
        }

        /// <summary>
        ///     Логика добавления репозиториев
        /// </summary>
        private void AddRepositories()
        {
            serviceCollection.AddTransient<IWeightingRepository, WeightingRepository>();
            serviceCollection.AddTransient<ICarRepository, CarRepository>();
        }
    }
}
