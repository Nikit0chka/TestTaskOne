using System.Threading.Tasks;

namespace TestTaskOne.Contracts;

/// <summary>
///     Контракт View-модели с асинхронной инициализацией
/// </summary>
internal interface IAsyncInitializableViewModel
{
    /// <summary>
    ///     Инициализировать асинхронно
    /// </summary>
    /// <param name="parameter">Параметры для инициализации</param>
    Task InitializeAsync(object? parameter);
}
