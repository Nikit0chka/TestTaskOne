using ErrorOr;
using Mediator;

namespace TestTask.Application.Weightings.UseCases.Delete;

/// <summary>
///     Команда удаления провески
/// </summary>
/// <param name="WeightingId">Идентификатор провески</param>
public readonly record struct DeleteWeightingCommand(int WeightingId)
    : ICommand<ErrorOr<Deleted>>;
