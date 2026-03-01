using System.Threading.Tasks;
using TestTaskOne.ViewModels;

namespace TestTaskOne.Contracts;

/// <summary>
///     Контракт сервиса навигации
/// </summary>
internal interface INavigationService
{
    /// <summary>
    ///     Совершить навигацию
    /// </summary>
    /// <param name="parameter">Параметр для передачи в viewModel</param>
    /// <typeparam name="T">Тип view-model вьюшки</typeparam>
    public Task NavigateToAsync<T>(object? parameter = null) where T : ViewModelBase;
}
