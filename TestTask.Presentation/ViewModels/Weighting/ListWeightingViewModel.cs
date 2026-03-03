using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TestTask.Application.Weightings.Dto;
using TestTask.Application.Weightings.Queries.GetList;
using TestTask.Application.Weightings.UseCases.Delete;
using TestTask.Application.Weightings.UseCases.Export;
using TestTaskOne.Contracts;
using TestTaskOne.Contracts.Dialog;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class ListWeightingViewModel(
    IMediator mediator,
    IMessageBoxPopupProvider messageBoxPopupProvider,
    IWindowService windowService,
    IFilePickerDialogService filePickerDialogService)
    : ViewModelBase, IAsyncInitializableViewModel
{
    [ObservableProperty] private ObservableCollection<ListWeightingModel> _weightings = [];

    public Task InitializeAsync(object? parameter)
    {
        return LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        var getWeightingListResult = await mediator.Send(new GetWeightingListQuery());

        if (getWeightingListResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error creating weighting, {getWeightingListResult.FirstError.Description}", ButtonEnum.Ok,
                Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        var newList = new ObservableCollection<ListWeightingModel>(
            getWeightingListResult.Value.Select(ListWeightingModel.Create));

        Weightings = newList;
    }

    [RelayCommand]
    private async Task GotoCreateWeightingPageAsync()
    {
        await windowService.ShowDialogAsync<CreateWeightingViewModel>();
        await LoadAsync();
    }

    [RelayCommand]
    private async Task GotoEditWeightingPageAsync(int weightingId)
    {
        await windowService.ShowDialogAsync<EditWeightingViewModel>(weightingId);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task ExportToFileAsync()
    {
        var filePath = await filePickerDialogService.OpenFilePickerAsync("Pick file to export");

        if (filePath is null)
            return;

        var exportWeightingResult = await mediator.Send(new ExportWeightingToFileCommand(filePath));

        var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
            $"Error exporting weighting, {exportWeightingResult.FirstError.Description}", ButtonEnum.Ok,
            Icon.Error);

        if (exportWeightingResult.IsError)
        {
            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        messageBox = MessageBoxManager.GetMessageBoxStandard("Success",
            "Successfully exported weightings", ButtonEnum.Ok,
            Icon.Success);

        await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
    }

    [RelayCommand]
    private async Task DeleteAsync(int weightingId)
    {
        var deleteWeightingResult =
            await mediator.Send(new DeleteWeightingCommand(weightingId));

        if (deleteWeightingResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error deleting weighting, {deleteWeightingResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        await LoadAsync();
    }
}
