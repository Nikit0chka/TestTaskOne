using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.AddWeightingTare;

/// <summary>
///     Обработчик команды добавления веса тары к провеске
/// </summary>
internal sealed class AddWeightingTareCommandHandler(
    IWeightingRepository weightingRepository,
    ILogger<AddWeightingTareCommandHandler> logger)
    : ICommandHandler<AddWeightingTareCommand, ErrorOr<Updated>>
{
    public async ValueTask<ErrorOr<Updated>> Handle(AddWeightingTareCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName}, WeightingId: {WeightingId}",
            nameof(AddWeightingTareCommand),
            command.WeightingId);

        try
        {
            var weighting = await weightingRepository.FindAsync(command.WeightingId, cancellationToken);
            if (weighting is null)
            {
                logger.LogWarning("Weighting with Id: {WeightingId} does not exist", command.WeightingId);
                return ApplicationErrors.WeightingErrors.NotFound;
            }

            var weightingTareCreateResult = WeightingTare.Create(command.WeightTareKg, DateTime.UtcNow);
            if (weightingTareCreateResult.IsError)
            {
                logger.LogWarning("Error creating weighting tare vo: {Error}", weightingTareCreateResult.FirstError);
                return weightingTareCreateResult.FirstError;
            }

            var addWeightingTareResult = weighting.AddWeightingTare(weightingTareCreateResult.Value);
            if (addWeightingTareResult.IsError)
            {
                logger.LogWarning("Error adding weighting tare: {Error}", addWeightingTareResult.FirstError);
                return addWeightingTareResult.FirstError;
            }

            await weightingRepository.UpdateAsync(weighting, cancellationToken);

            logger.LogInformation("{CommandName} handled successfully. Weighting tare added",
                nameof(AddWeightingTareCommand));

            return Result.Updated;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {CommandName}", nameof(AddWeightingTareCommand));
            return Error.Unexpected(description: "Unexpected error while adding weighting tare");
        }
    }
}
