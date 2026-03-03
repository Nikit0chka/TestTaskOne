using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Application.Cars.Dto;
using TestTask.Domain.CarAggregate;

namespace TestTask.Application.Cars.Queries.GetSelectList;

/// <summary>
///     Обработчик запроса получения списка взвешиваний
/// </summary>
internal sealed class GetCarSelectListQueryHandler(
    ICarRepository carRepository,
    ILogger<GetCarSelectListQueryHandler> logger)
    : IQueryHandler<GetCarSelectListQuery, ErrorOr<IReadOnlyCollection<CarSelectListModel>>>
{
    public async ValueTask<ErrorOr<IReadOnlyCollection<CarSelectListModel>>> Handle(GetCarSelectListQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {QueryName}", nameof(GetCarSelectListQuery));

        try
        {
            var weightingList = await carRepository.GetList(cancellationToken);

            logger.LogInformation("{QueryName} handled successfully", nameof(GetCarSelectListQuery));

            return weightingList.Select(CarSelectListModel.Create).ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {QueryName}", nameof(GetCarSelectListQuery));
            return Error.Unexpected(description: "Unexpected error while getting car list");
        }
    }
}
