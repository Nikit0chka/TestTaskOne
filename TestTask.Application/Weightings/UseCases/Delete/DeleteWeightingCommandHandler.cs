using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Delete;

/// <summary>
///     Обработчик команды удаления провески
/// </summary>
internal sealed class DeleteWeightingCommandHandler(
    IWeightingRepository weightingRepository,
    ILogger<DeleteWeightingCommandHandler> logger)
    : ICommandHandler<DeleteWeightingCommand, ErrorOr<Deleted>>
{
    public async ValueTask<ErrorOr<Deleted>> Handle(DeleteWeightingCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName}", nameof(DeleteWeightingCommand));

        try
        {
            var weighting = await weightingRepository.FindAsync(command.WeightingId, cancellationToken);
            if (weighting is null)
            {
                logger.LogWarning("Weighting with Id: {WeightingId} does not exist", command.WeightingId);
                return ApplicationErrors.WeightingErrors.NotFound;
            }

            await weightingRepository.DeleteAsync(weighting, cancellationToken);

            logger.LogInformation("{CommandName} handled successfully. Weighting deleted",
                nameof(DeleteWeightingCommand));

            return Result.Deleted;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {CommandName}", nameof(DeleteWeightingCommand));
            return Error.Unexpected(description: "Unexpected error while deleting weighting");
        }
    }
}
