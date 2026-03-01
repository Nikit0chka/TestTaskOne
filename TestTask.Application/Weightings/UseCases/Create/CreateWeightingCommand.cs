using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.Create;

/// <summary>
///     Команда создания провески
/// </summary>
/// <param name="CarNumber">Номер машины</param>
/// <param name="WeightGrossKg">Вес брутто</param>
public readonly record struct CreateWeightingCommand(string CarNumber, double WeightGrossKg)
    : ICommand<ErrorOr<Created>>;
