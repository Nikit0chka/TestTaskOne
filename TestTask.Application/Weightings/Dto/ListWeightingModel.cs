using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.Dto;

/// <summary>
///     Модель провески для списка
/// </summary>
public readonly record struct ListWeightingModel
{
    public int WeightingId { get; init; }
    public string CarNumber { get; init; }
    public double WeightingGross { get; init; }
    public double? WeightingNet { get; init; }
    public double? WeightingTare { get; init; }
    public DateTime WeightingGrossDate { get; init; }
    public DateTime? WeightingTareDate { get; init; }

    /// <summary>
    ///     Основной конструктор создания
    /// </summary>
    private ListWeightingModel(int weightingId, string carNumber, double weightingGross, double? weightingNet,
        double? weightingTare, DateTime weightingGrossDate, DateTime? weightingTareDate)
    {
        WeightingId = weightingId;
        CarNumber = carNumber;
        WeightingGross = weightingGross;
        WeightingNet = weightingNet;
        WeightingTare = weightingTare;
        WeightingGrossDate = weightingGrossDate;
        WeightingTareDate = weightingTareDate;
    }

    /// <summary>
    ///     Фабричный метод создания
    /// </summary>
    /// <param name="weighting">Провеска</param>
    public static ListWeightingModel Create(Weighting weighting)
    {
        return new ListWeightingModel(weighting.Id, weighting.Car.Number, weighting.WeightingGross.WeightKg,
            weighting.WeightNetKg, weighting.WeightingTare?.WeightKg, weighting.WeightingGross.WeightTime,
            weighting.WeightingTare?.WeightTime);
    }
}
