using Mediator;
using ErrorOr;
using TestTask.Application.Cars.Dto;

namespace TestTask.Application.Cars.Queries.GetSelectList;

public readonly record struct GetCarSelectListQuery : IQuery<ErrorOr<IReadOnlyCollection<CarSelectListModel>>>;
