using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.Export;

/// <summary>
///     Команда импорта провесок из файла
/// </summary>
/// <param name="FilePath">Путь к файлу</param>
public readonly record struct ExportWeightingToFileCommand(string FilePath)
    : ICommand<ErrorOr<Success>>;
