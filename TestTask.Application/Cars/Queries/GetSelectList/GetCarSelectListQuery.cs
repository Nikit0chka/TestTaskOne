using Mediator;
using ErrorOr;
using TestTask.Application.Cars.Dto;

namespace TestTask.Application.Cars.Queries.GetSelectList;

/// <summary>
///     Запрос получения списка машин
/// </summary>
public readonly record struct GetCarSelectListQuery : IQuery<ErrorOr<IReadOnlyCollection<CarSelectListModel>>>;
