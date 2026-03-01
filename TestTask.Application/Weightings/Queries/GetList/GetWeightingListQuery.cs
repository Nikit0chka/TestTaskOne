using Mediator;
using TestTask.Domain.WeightingAggregate;
using ErrorOr;

namespace TestTask.Application.Weightings.Queries.GetList;

/// <summary>
///     Запрос получения списка взвешиваний
/// </summary>
public readonly record struct GetWeightingListQuery : IQuery<ErrorOr<IReadOnlyCollection<Weighting>>>;
