namespace TestTask.Domain.Services;

/// <summary>
///     Сервис подсчета веса нет провески
/// </summary>
public static class WeightingNetCalculator
{
    /// <summary>
    ///     Логика подсчета веса нет провески
    /// </summary>
    /// <param name="weightGross">Вес брутто</param>
    /// <param name="weightTare">Вес нет</param>
    public static double CalculateWeightNet(double weightGross, double weightTare)
    {
        return weightGross - weightTare;
    }
}
