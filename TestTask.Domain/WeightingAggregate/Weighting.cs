using ErrorOr;
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
    ///     Номер машины
    /// </summary>
    public CarNumber CarNumber { get; private set; }

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
    /// <param name="carNumber">Номер машины</param>
    /// <param name="weightingGross"></param>
    public Weighting(CarNumber carNumber, WeightingGross weightingGross)
    {
        CarNumber = carNumber;
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
    /// <param name="carNumber">Номер машины</param>
    /// <param name="weightingGross">Взвешивание брутто</param>
    /// <param name="weightingTare">Взвешивание тары</param>
    public ErrorOr<Updated> Update(CarNumber carNumber, WeightingGross weightingGross, WeightingTare? weightingTare)
    {
        if (weightingTare is not null)
        {
            if (weightingTare.WeightKg >= weightingGross.WeightKg)
                return DomainErrors.WeightingAggregateDomainErrors.WeightTareMustBeLessWeightGross;

            WeightNetKg = weightingGross.WeightKg - weightingTare.WeightKg;
        }

        WeightingTare = weightingTare;
        WeightingGross = weightingGross;
        CarNumber = carNumber;

        return Result.Updated;
    }
}
