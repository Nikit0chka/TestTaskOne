using ErrorOr;

namespace TestTask.Application.Weightings;

/// <summary>
///     Ошибки Application слоя
/// </summary>
internal static class ApplicationErrors
{
    /// <summary>
    ///     Ошибки связанные с провесками
    /// </summary>
    public static class WeightingErrors
    {
        public static readonly Error NotFound = Error.NotFound(description: "Взвешивание не найдено");
    }

    /// <summary>
    ///     Ошибки связанные с машиной
    /// </summary>
    public static class CarErrors
    {
        public static readonly Error NotFound = Error.NotFound(description: "Автомобиль не найден");
    }
}
