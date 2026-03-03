namespace TestTaskOne.Contracts.Dialog;

/// <summary>
///     Dto для фильтрации файлов
/// </summary>
/// <param name="Name">Имя файла</param>
/// <param name="Patterns">Паттерны</param>
public abstract record FileFilter(string Name, string[] Patterns);
