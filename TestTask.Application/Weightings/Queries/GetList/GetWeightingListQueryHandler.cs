using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.Queries.GetList;

/// <summary>
///     Обработчик запроса получения списка взвешиваний
/// </summary>
internal sealed class GetWeightingListQueryHandler(
    IWeightingRepository weightingRepository,
    ILogger<GetWeightingListQueryHandler> logger)
    : IQueryHandler<GetWeightingListQuery, ErrorOr<IReadOnlyCollection<Weighting>>>
{
    public async ValueTask<ErrorOr<IReadOnlyCollection<Weighting>>> Handle(GetWeightingListQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {QueryName}", nameof(GetWeightingListQuery));

        try
        {
            var weightingList = await weightingRepository.GetList(cancellationToken);

            logger.LogInformation("{QueryName} handled successfully", nameof(GetWeightingListQuery));

            return weightingList;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {QueryName}", nameof(GetWeightingListQuery));
            return Error.Unexpected(description: "Unexpected error while getting list weighting");
        }
    }
}
