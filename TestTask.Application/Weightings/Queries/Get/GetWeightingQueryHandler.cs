using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.Queries.Get;

/// <summary>
///     Обработчик запроса получения провески
/// </summary>
internal sealed class GetWeightingQueryHandler(
    IWeightingRepository weightingRepository,
    ILogger<GetWeightingQueryHandler> logger)
    : IQueryHandler<GetWeightingQuery, ErrorOr<Weighting>>
{
    public async ValueTask<ErrorOr<Weighting>> Handle(GetWeightingQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {QueryName}", nameof(GetWeightingQuery));

        try
        {
            var weighting = await weightingRepository.FindAsync(query.WeightingId, cancellationToken);

            if (weighting != null)
            {
                logger.LogInformation("{QueryName} handled successfully", nameof(GetWeightingQuery));
                return weighting;
            }

            logger.LogWarning("Weighting with Id: {WeightingId} not found:", query.WeightingId);
            return Error.NotFound(description: "Weighting with Id: {WeightingId} not found");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {QueryName}", nameof(GetWeightingQuery));
            return Error.Unexpected(description: "Unexpected error while getting weighting");
        }
    }
}
