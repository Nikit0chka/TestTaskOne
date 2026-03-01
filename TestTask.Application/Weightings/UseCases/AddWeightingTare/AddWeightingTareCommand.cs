using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.AddWeightingTare;

/// <summary>
///     Команда добавления веса тары к провеске
/// </summary>
/// <param name="WeightingId">Идентификатор провески</param>
/// <param name="WeightTareKg">Вес тары</param>
public readonly record struct AddWeightingTareCommand(int WeightingId, double WeightTareKg)
    : ICommand<ErrorOr<Updated>>;
