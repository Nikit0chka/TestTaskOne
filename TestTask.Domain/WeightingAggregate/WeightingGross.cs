using ErrorOr;

namespace TestTask.Domain.WeightingAggregate;

/// <summary>
///     Объект значение веса брутто
/// </summary>
public sealed record WeightingGross
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
    /// <param name="weightTime">Время взвешивания</param>
    private WeightingGross(double weightKg, DateTime weightTime)
    {
        if (weightKg <= 0)
            throw new AbandonedMutexException();

        WeightKg = weightKg;
        WeightTime = weightTime;
    }

    /// <summary>
    ///     Ef-core конструктор
    /// </summary>
    private WeightingGross()
    {
    }

    /// <summary>
    ///     Фабричный метод создания
    /// </summary>
    /// <param name="weightKg">Вес в кг</param>
    /// <param name="weightTime">Время взвешивания</param>
    public static ErrorOr<WeightingGross> Create(double weightKg, DateTime weightTime)
    {
        if (weightKg <= 0)
            return DomainErrors.WeightingAggregateDomainErrors.WeightingGrossWeightCanNotBeLessThanOne;

        return new WeightingGross(weightKg, weightTime);
    }
}
