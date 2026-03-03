using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.Create;

/// <summary>
///     Команда создания провески
/// </summary>
/// <param name="CarId">Идентификатор машины</param>
/// <param name="WeightGrossKg">Вес брутто</param>
public readonly record struct CreateWeightingCommand(int CarId, double WeightGrossKg)
    : ICommand<ErrorOr<Created>>;
