using TestTask.Application.Weightings.Dto;

namespace TestTask.Application.Contracts;

/// <summary>
///     Контракт сервиса провесок для работы с файлами
/// </summary>
public interface IWeightingFileService
{
    /// <summary>
    ///     Записать провески в файла
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    /// <param name="weightings">Провески для записи</param>
    /// <param name="cancellationToken">Токен для отмены асинхронной операции</param>
    public Task WriteToFile(string path, IReadOnlyCollection<ListWeightingModel> weightings,
        CancellationToken cancellationToken);
}
