using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ErrorOr;
using Mediator;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Application.Weightings.Dto;
using TestTask.Application.Weightings.Queries.GetList;
using TestTask.Application.Weightings.UseCases.Delete;
using TestTask.Application.Weightings.UseCases.Export;
using TestTaskOne.Contracts;
using TestTaskOne.Contracts.Dialog;
using TestTaskOne.Models;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class ListWeightingViewModel(
    IMediator mediator,
    IMessageBoxPopupProvider messageBoxPopupProvider,
    IWindowService windowService,
    IFilePickerDialogService filePickerDialogService)
    : ViewModelBase, IAsyncInitializableViewModel
{
    [ObservableProperty] private ObservableCollection<ListWeightingModel> _weightings = [];

    [ObservableProperty]
    private WeightingChartViewModel _weightingChartViewModel = new();



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
        var list = getWeightingListResult.Value.Select(ListWeightingModel.Create).ToList();
        var chartData = list
            .GroupBy(x => x.WeightingGrossDate.Date) 
            .Select(g => new GrafModels
            {
                Date = g.Key,
                TotalWeight = g.Sum(x => x.WeightingGross), 
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        WeightingChartViewModel.ChartData = new ObservableCollection<GrafModels>(chartData);
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
