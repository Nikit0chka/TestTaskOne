using System;
using System.Linq;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TestTask.Application.Extensions;
using TestTask.Infrastructure.Extensions;
using TestTaskOne.Contracts;
using TestTaskOne.Extensions;
using TestTaskOne.ViewModels;
using TestTaskOne.ViewModels.Weighting;
using AvaloniaApp = Avalonia.Application;
using MainWindow = TestTaskOne.Windows.MainWindow;

namespace TestTaskOne;

public sealed class App : AvaloniaApp
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            DisableAvaloniaDataAnnotationValidation();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            using var loggerFactory = LoggerFactory.Create(static loggingBuilder => loggingBuilder.AddSerilog());

            var logger = loggerFactory.CreateLogger<App>();

            ConfigureServices(services, configuration, logger);

            var serviceProvider = services.BuildServiceProvider();

            desktop.MainWindow = serviceProvider.GetRequiredService<MainWindow>();
            desktop.MainWindow.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();

            var navigationService = serviceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateToAsync<ListWeightingViewModel>();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration,
        ILogger<App> logger)
    {
        services.AddPresentationServices(logger);
        services.AddInfrastructureServices(configuration, logger);
        services.AddApplicationServices(logger);
    }
}
