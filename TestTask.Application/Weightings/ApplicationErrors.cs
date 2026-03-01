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
        public static readonly Error NotFound = Error.NotFound(description: "Weighting not found");
    }
}
