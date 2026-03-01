using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Create;

/// <summary>
///     Обработчик команды создания провески
/// </summary>
internal sealed class CreateWeightingCommandHandler(
    IWeightingRepository weightingRepository,
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

            var carNumber = CarNumber.Create(command.CarNumber);
            if (carNumber.IsError)
            {
                logger.LogWarning("Error creating car number vo: {Error}", carNumber.FirstError);
                return carNumber.FirstError;
            }

            var weighting = new Weighting(carNumber.Value, weightingGross.Value);

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
