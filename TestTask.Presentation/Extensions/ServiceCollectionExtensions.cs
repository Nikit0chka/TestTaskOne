using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTaskOne.Contracts;
using TestTaskOne.Contracts.Dialog;
using TestTaskOne.Services;
using TestTaskOne.ViewModels;
using TestTaskOne.ViewModels.Weighting;
using MainWindow = TestTaskOne.Windows.MainWindow;

namespace TestTaskOne.Extensions;

/// <summary>
///     Методы-расширения для работы с ioc-контейнером.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <param name="serviceCollection"> Ioc-контейнер. </param>
    extension(IServiceCollection serviceCollection)
    {
        /// <summary>
        ///     Зарегистрировать presentation сервисы в ioc-контейнер.
        /// </summary>
        /// <param name="logger"> Логер. </param>
        public void AddPresentationServices(ILogger logger)
        {
            logger.LogInformation("Регистрация presentation сервисов...");

            serviceCollection.AddViewModels();
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddServices();

            logger.LogInformation("Presentation сервисы зарегистрированы");
        }

        /// <summary>
        ///     Зарегистрировать вью-модели в ioc-контейнере
        /// </summary>
        private void AddViewModels()
        {
            serviceCollection.AddTransient<CreateWeightingViewModel>();
            serviceCollection.AddTransient<ListWeightingViewModel>();
            serviceCollection.AddTransient<EditWeightingViewModel>();
            serviceCollection.AddTransient<AddWeightingTareWeightingViewModel>();

            serviceCollection.AddSingleton<MainWindowViewModel>();
        }

        /// <summary>
        ///     Регистрация основных сервисов в ioc-контейнере
        /// </summary>
        private void AddServices()
        {
            serviceCollection.AddSingleton<IWindowService, WindowService>();
            serviceCollection.AddSingleton<IMessageBoxPopupProvider, MessageBoxPopupProvider>();
            serviceCollection.AddSingleton<INavigationService, NavigationService>();
            serviceCollection.AddSingleton<IFilePickerDialogService, FilePickerDialogService>();
        }
    }
}
