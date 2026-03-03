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
    /// <param name="weightNet">Вес нет</param>
    public static double CalculateWeightNet(double weightGross, double weightNet)
    {
        return weightGross - weightNet;
    }
}
