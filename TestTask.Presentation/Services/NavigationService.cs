using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestTaskOne.Contracts;
using TestTaskOne.ViewModels;

namespace TestTaskOne.Services;

/// <inheritdoc />
internal sealed class NavigationService(MainWindowViewModel mainWindowViewModel, IServiceProvider serviceProvider)
    : INavigationService
{
    public Task NavigateToAsync<T>(object? parameter = null) where T : ViewModelBase
    {
        var viewModel = serviceProvider.GetRequiredService<T>();
        mainWindowViewModel.CurrentViewModel = viewModel;

        if (viewModel is IAsyncInitializableViewModel initializableViewModel)
            return initializableViewModel.InitializeAsync(parameter);

        return Task.CompletedTask;
    }
}
