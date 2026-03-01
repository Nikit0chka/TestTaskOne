using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.Update;

/// <summary>
///     Команда обновления провески
/// </summary>
/// <param name="WeightingId">Идентификатор провески</param>
/// <param name="CarNumber">Номер машины</param>
/// <param name="WeightGrossKg">Вес брутто кг</param>
/// <param name="WeightTareKg">Вес тары кг</param>
public readonly record struct UpdateWeightingCommand(
    int WeightingId,
    string CarNumber,
    double WeightGrossKg,
    double WeightTareKg)
    : ICommand<ErrorOr<Created>>;
