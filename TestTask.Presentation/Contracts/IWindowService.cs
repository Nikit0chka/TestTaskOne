using System.Threading.Tasks;
using Avalonia.Controls;
using TestTaskOne.ViewModels;

namespace TestTaskOne.Contracts;

/// <summary>
///     Контракт сервиса окон
/// </summary>
public interface IWindowService
{
    /// <summary>
    ///     Открыть окно как модальное
    /// </summary>
    /// <param name="parameter">Параметр для передачи</param>
    /// <typeparam name="TViewModel">Тип вью-модели окна</typeparam>
    Task ShowDialogAsync<TViewModel>(object? parameter = null) where TViewModel : ViewModelBase;

    /// <summary>
    ///     Закрыть окно
    /// </summary>
    /// <param name="viewModel">Вью-модель окна</param>
    void CloseWindow(ViewModelBase viewModel);

    /// <summary>
    ///     Получить окно по вью-модели
    /// </summary>
    /// <param name="viewModel">Вью-модель для получения окна</param>
    Window? GetWindowForViewModel(ViewModelBase viewModel);

    /// <summary>
    ///     Получить активное окно
    /// </summary>
    Window? GetActiveWindow();
}
