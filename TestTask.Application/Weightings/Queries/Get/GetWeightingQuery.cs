using ErrorOr;
using Mediator;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.Queries.Get;

/// <summary>
///     Запрос получения провески
/// </summary>
/// <param name="WeightingId">Идентификатор провески</param>
public readonly record struct GetWeightingQuery(int WeightingId) : IQuery<ErrorOr<Weighting>>;
