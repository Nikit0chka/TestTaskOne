using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTaskOne.Contracts.Dialog;

/// <summary>
///     Контракт сервиса работы с окном выбора файлов
/// </summary>
public interface IFilePickerDialogService
{
    /// <summary>
    ///     Открывает диалог выбора одного файла.
    /// </summary>
    /// <param name="title">Заголовок окна.</param>
    /// <param name="allowMultiple">
    ///     Разрешить выбор нескольких файлов (влияет только на заголовок, но метод вернёт первый
    ///     выбранный).
    /// </param>
    /// <param name="filters">Список фильтров для отображаемых файлов.</param>
    /// <returns>Полный путь к выбранному файлу или null, если пользователь отменил выбор.</returns>
    Task<string?> OpenFilePickerAsync(
        string title,
        bool allowMultiple = false,
        IEnumerable<FileFilter>? filters = null);
}
