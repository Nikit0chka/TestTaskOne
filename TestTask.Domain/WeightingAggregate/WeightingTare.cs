using ErrorOr;

namespace TestTask.Domain.WeightingAggregate;

/// <summary>
///     Объект значение веса тары
/// </summary>
public sealed record WeightingTare
{
    /// <summary>
    ///     Вес в кг
    /// </summary>
    public double WeightKg { get; private init; }

    /// <summary>
    ///     Время взвешивания
    /// </summary>
    public DateTime WeightTime { get; private init; }

    /// <summary>
    ///     Основной конструктор
    /// </summary>
    /// <param name="weightKg">Вес в кг</param>
    /// <param name="weightTime">Дата взвешивания</param>
    private WeightingTare(double weightKg, DateTime weightTime)
    {
        WeightKg = weightKg;
        WeightTime = weightTime;
    }

    /// <summary>
    ///     Ef-core конструктор
    /// </summary>
    private WeightingTare()
    {
    }

    /// <summary>
    ///     Фабричный метод создания
    /// </summary>
    /// <param name="weightKg">Вес в кг</param>
    /// <param name="weightTime">Дата взвешивания</param>
    public static ErrorOr<WeightingTare> Create(double weightKg, DateTime weightTime)
    {
        if (weightKg <= 0)
            return DomainErrors.WeightingAggregateDomainErrors.WeightingTareWeightCanNotBeLessThanOne;

        return new WeightingTare(weightKg, weightTime);
    }
}
