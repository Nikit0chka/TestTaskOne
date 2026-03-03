using ErrorOr;
using Mediator;
using Microsoft.Extensions.Logging;
using TestTask.Application.Contracts;
using TestTask.Application.Weightings.Dto;
using TestTask.Domain.WeightingAggregate;

namespace TestTask.Application.Weightings.UseCases.Export;

/// <summary>
///     Обработчик команды импорта провесок из файла
/// </summary>
internal sealed class
    ExportWeightingToFileCommandHandler(
        IWeightingFileService weightingFileService,
        IWeightingRepository weightingRepository,
        ILogger<ExportWeightingToFileCommandHandler> logger)
    : ICommandHandler<ExportWeightingToFileCommand, ErrorOr<Success>>
{
    public async ValueTask<ErrorOr<Success>> Handle(ExportWeightingToFileCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {CommandName}", nameof(ExportWeightingToFileCommand));

        try
        {
            var weightings = await weightingRepository.GetList(cancellationToken);

            await weightingFileService.WriteToFile(command.FilePath,
                weightings.Select(ListWeightingModel.Create).ToList(), cancellationToken);

            logger.LogInformation("{CommandName} handled successfully. {WeightingsCount} weightings imported",
                nameof(ExportWeightingToFileCommand), weightings.Count);

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while handling {CommandName}", nameof(ExportWeightingToFileCommand));
            return Error.Unexpected(description: "Unexpected error while exporting weightings");
        }
    }
}
