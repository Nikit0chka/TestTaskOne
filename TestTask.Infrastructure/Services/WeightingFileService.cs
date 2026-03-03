using System.Text.Json;
using TestTask.Application.Contracts;
using TestTask.Application.Weightings.Dto;

namespace TestTask.Infrastructure.Services;

/// <inheritdoc />
internal sealed class WeightingFileService : IWeightingFileService
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public Task WriteToFile(string path, IReadOnlyCollection<ListWeightingModel> weightings,
        CancellationToken cancellationToken)
    {
        var serializedContent = JsonSerializer.Serialize(weightings, _jsonSerializerOptions);

        return File.WriteAllTextAsync(path, serializedContent, cancellationToken);
    }
}
