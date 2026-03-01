using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Update;

/// <summary>
///     Обработчик команды обновления провески
/// </summary>
internal sealed class UpdateWeightingCommandHandler(
    IWeightingRepository weightingRepository,
    ILogger<UpdateWeightingCommandHandler> logger)
    : ICommandHandler<UpdateWeightingCommand, ErrorOr<Created>>
{
    public async ValueTask<ErrorOr<Created>> Handle(UpdateWeightingCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName}, WeightingId: {WeightingId}",
            nameof(UpdateWeightingCommand),
            command.WeightingId);

        try
        {
            var weighting = await weightingRepository.FindAsync(command.WeightingId, cancellationToken);
            if (weighting is null)
            {
                logger.LogWarning("Weighting with Id: {WeightingId} does not exist", command.WeightingId);
                return ApplicationErrors.WeightingErrors.NotFound;
            }

            var weightingGrossCreateResult = WeightingGross.Create(command.WeightGrossKg, DateTime.UtcNow);
            if (weightingGrossCreateResult.IsError)
            {
                logger.LogWarning("Error creating weighting gross vo: {Error}", weightingGrossCreateResult.FirstError);
                return weightingGrossCreateResult.FirstError;
            }

            var carNumberCreateResult = CarNumber.Create(command.CarNumber);
            if (carNumberCreateResult.IsError)
            {
                logger.LogWarning("Error creating car number vo: {Error}", carNumberCreateResult.FirstError);
                return carNumberCreateResult.FirstError;
            }

            WeightingTare? weightingTare = null;
            if (command.WeightTareKg != 0)
            {
                var weightingTareCreateResult = WeightingTare.Create(command.WeightTareKg, DateTime.UtcNow);
                if (weightingTareCreateResult.IsError)
                {
                    logger.LogWarning("Error creating weighting tare vo: {Error}",
                        weightingTareCreateResult.FirstError);

                    return weightingTareCreateResult.FirstError;
                }

                weightingTare = weightingTareCreateResult.Value;
            }

            var weightingUpdateResult = weighting.Update(carNumberCreateResult.Value,
                weightingGrossCreateResult.Value,
                weightingTare);

            if (weightingUpdateResult.IsError)
            {
                logger.LogWarning("Error updating weighting: {Error}", carNumberCreateResult.FirstError);
                return carNumberCreateResult.FirstError;
            }

            await weightingRepository.UpdateAsync(weighting, cancellationToken);

            logger.LogInformation("{CommandName} handled successfully. Weighting updated",
                nameof(UpdateWeightingCommand));

            return Result.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {CommandName}", nameof(UpdateWeightingCommand));
            return Error.Unexpected(description: "Unexpected error while updating weighting");
        }
    }
}
