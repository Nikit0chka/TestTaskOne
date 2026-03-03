using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.CarAggregate;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Create;

/// <summary>
///     Обработчик команды создания провески
/// </summary>
internal sealed class CreateWeightingCommandHandler(
    IWeightingRepository weightingRepository,
    ICarRepository carRepository,
    ILogger<CreateWeightingCommandHandler> logger)
    : ICommandHandler<CreateWeightingCommand, ErrorOr<Created>>
{
    public async ValueTask<ErrorOr<Created>> Handle(CreateWeightingCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName}", nameof(CreateWeightingCommand));

        try
        {
            var weightingGross = WeightingGross.Create(command.WeightGrossKg, DateTime.UtcNow);
            if (weightingGross.IsError)
            {
                logger.LogWarning("Error creating weighting gross vo: {Error}", weightingGross.FirstError);
                return weightingGross.FirstError;
            }

            var car = await carRepository.FindAsync(command.CarId, cancellationToken);
            if (car is null)
            {
                logger.LogWarning("Car with id: {Id} not found", command.CarId);
                return ApplicationErrors.CarErrors.NotFound;
            }

            var weighting = new Weighting(car, weightingGross.Value);

            await weightingRepository.AddAsync(weighting, cancellationToken);

            logger.LogInformation("{CommandName} handled successfully. Weighting created",
                nameof(CreateWeightingCommand));

            return Result.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {CommandName}", nameof(CreateWeightingCommand));
            return Error.Unexpected(description: "Unexpected error while creating weighting");
        }
    }
}
