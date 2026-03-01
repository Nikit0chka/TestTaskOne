using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using TestTaskOne.Contracts;
using TestTaskOne.ViewModels;
using MainWindow = TestTaskOne.Windows.MainWindow;

namespace TestTaskOne.Services;

/// <inheritdoc />
internal sealed class WindowService(MainWindow mainWindow, IServiceProvider serviceProvider) : IWindowService
{
    private readonly Dictionary<ViewModelBase, Window> _openWindows = new();

    public async Task ShowDialogAsync<TViewModel>(object? parameter = null) where TViewModel : ViewModelBase
    {
        var viewModel = serviceProvider.GetRequiredService<TViewModel>();
        var window = CreateWindowForViewModel(viewModel);
        if (window == null)
            throw new InvalidOperationException($"Cannot find window for ViewModel {typeof(TViewModel).Name}");

        window.DataContext = viewModel;
        _openWindows[viewModel] = window;

        if (viewModel is IAsyncInitializableViewModel initializableViewModel)
            window.Opened += async (_, _) => { await initializableViewModel.InitializeAsync(parameter); };

        var owner = GetActiveWindow() ?? mainWindow;
        await window.ShowDialog(owner);

        _openWindows.Remove(viewModel);
    }

    public void CloseWindow(ViewModelBase viewModel)
    {
        if (!_openWindows.TryGetValue(viewModel, out var window))
            return;

        window.Close();
        _openWindows.Remove(viewModel);
    }

    public Window? GetWindowForViewModel(ViewModelBase viewModel)
    {
        _openWindows.TryGetValue(viewModel, out var window);
        return window;
    }

    public Window? GetActiveWindow()
    {
        var active = _openWindows.Values.FirstOrDefault(w => w.IsActive);
        if (active != null)
            return active;

        if (mainWindow.IsActive)
            return mainWindow;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;

        return mainWindow;
    }

    private static Window? CreateWindowForViewModel(ViewModelBase viewModel)
    {
        var viewModelType = viewModel.GetType();

        var windowTypeName = viewModelType.FullName!.Replace("ViewModel", "Window");
        var windowType = Type.GetType(windowTypeName);

        if (windowType == null)
            return null;

        return (Window)Activator.CreateInstance(windowType)!;
    }
}
