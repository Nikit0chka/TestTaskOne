using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.CarAggregate;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Update;

/// <summary>
///     Обработчик команды обновления провески
/// </summary>
internal sealed class UpdateWeightingCommandHandler(
    IWeightingRepository weightingRepository,
    ICarRepository carRepository,
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

            var car = await carRepository.FindAsync(command.CarId, cancellationToken);
            if (car is null)
            {
                logger.LogWarning("Car with id: {Id} not found", command.CarId);
                return ApplicationErrors.CarErrors.NotFound;
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

            var weightingUpdateResult = weighting.Update(car,
                weightingGrossCreateResult.Value,
                weightingTare);

            if (weightingUpdateResult.IsError)
            {
                logger.LogWarning("Error updating weighting: {Error}", weightingUpdateResult.FirstError);
                return weightingUpdateResult.FirstError;
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
