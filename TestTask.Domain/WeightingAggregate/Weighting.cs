using ErrorOr;
using TestTask.Domain.CarAggregate;
using TestTask.Domain.Services;

namespace TestTask.Domain.WeightingAggregate;

/// <summary>
///     Агрегат провески
/// </summary>
public sealed class Weighting
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    public int Id { get; private init; }

    /// <summary>
    ///     Машина
    /// </summary>
    public Car Car { get; private set; }

    /// <summary>
    ///     Взвешивание брутто
    /// </summary>
    public WeightingGross WeightingGross { get; private set; }

    /// <summary>
    ///     Вес тары
    /// </summary>
    public WeightingTare? WeightingTare { get; private set; }

    /// <summary>
    ///     Дата создания
    /// </summary>
    public DateTime CreatedOn { get; private init; }

    /// <summary>
    ///     Вес нет
    /// </summary>
    public double? WeightNetKg { get; private set; }

    /// <summary>
    ///     Основной конструктор
    /// </summary>
    /// <param name="car">Машина</param>
    /// <param name="weightingGross"></param>
    public Weighting(Car car, WeightingGross weightingGross)
    {
        Car = car;
        WeightingGross = weightingGross;

        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    ///     Ef-конструктор
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Weighting()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    /// <summary>
    ///     Добавить взвешивание тары
    /// </summary>
    /// <param name="weightingTare"></param>
    public ErrorOr<Updated> AddWeightingTare(WeightingTare weightingTare)
    {
        if (WeightingTare is not null)
            return DomainErrors.WeightingAggregateDomainErrors.CanNotAddWeightingTareWhenItAlreadyExist;

        if (weightingTare.WeightKg >= WeightingGross.WeightKg)
            return DomainErrors.WeightingAggregateDomainErrors.WeightTareMustBeLessWeightGross;

        WeightingTare = weightingTare;
        WeightNetKg = WeightingNetCalculator.CalculateWeightNet(WeightingGross.WeightKg, weightingTare.WeightKg);

        return Result.Updated;
    }

    /// <summary>
    ///     Логика обновления
    /// </summary>
    /// <param name="car">Машина</param>
    /// <param name="weightingGross">Взвешивание брутто</param>
    /// <param name="weightingTare">Взвешивание тары</param>
    public ErrorOr<Updated> Update(Car car, WeightingGross weightingGross, WeightingTare? weightingTare)
    {
        if (weightingTare is not null)
        {
            if (weightingTare.WeightKg >= weightingGross.WeightKg)
                return DomainErrors.WeightingAggregateDomainErrors.WeightTareMustBeLessWeightGross;

            WeightNetKg = weightingGross.WeightKg - weightingTare.WeightKg;
        }

        WeightingTare = weightingTare;
        WeightingGross = weightingGross;
        Car = car;

        return Result.Updated;
    }
}
