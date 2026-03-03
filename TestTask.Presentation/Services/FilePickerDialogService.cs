using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using TestTaskOne.Contracts;
using TestTaskOne.Contracts.Dialog;

namespace TestTaskOne.Services;

/// <inheritdoc />
internal sealed class FilePickerDialogService(IWindowService windowService) : IFilePickerDialogService
{
    public async Task<string?> OpenFilePickerAsync(
        string title,
        bool allowMultiple = false,
        IEnumerable<FileFilter>? filters = null)
    {
        var topLevel = windowService.GetActiveWindow();
        if (topLevel == null)
            return null;

        var options = new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = allowMultiple,
            FileTypeFilter = ConvertFilters(filters)
        };

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
        return files.Count > 0 ? files[0].Path.LocalPath : null;
    }

    private static IReadOnlyList<FilePickerFileType>? ConvertFilters(IEnumerable<FileFilter>? filters)
    {
        return filters?.Select(ConvertFilter).ToList();
    }

    private static FilePickerFileType ConvertFilter(FileFilter filter)
    {
        return new FilePickerFileType(filter.Name)
        {
            Patterns = filter.Patterns
        };
    }
}
