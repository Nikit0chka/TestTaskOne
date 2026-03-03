using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TestTask.Application.Cars.Dto;
using TestTask.Application.Cars.Queries.GetSelectList;
using TestTask.Application.Weightings.Queries.Get;
using TestTask.Application.Weightings.UseCases.Update;
using TestTask.Domain.Services;
using TestTaskOne.Contracts;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class EditWeightingViewModel(
    IMediator mediator,
    IWindowService windowService,
    IMessageBoxPopupProvider messageBoxPopupProvider) : ViewModelBase, IAsyncInitializableViewModel
{
    private int _weightingId;

    [ObservableProperty] private ObservableCollection<CarSelectListModel> _carSelectList = [];
    [ObservableProperty] private CarSelectListModel _selectedCar;
    [ObservableProperty] private double _weightGross;
    [ObservableProperty] private double _weightTare;
    [ObservableProperty] private double _weightNet;

    public async Task InitializeAsync(object? parameter)
    {
        if (parameter is int weightingId)
            await LoadAsync(weightingId);
        else
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                "Error initializing page", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        var updateWeightingResult =
            await mediator.Send(new UpdateWeightingCommand(_weightingId, SelectedCar.Id, WeightGross, WeightTare));

        if (updateWeightingResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error updating weighting, {updateWeightingResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        windowService.CloseWindow(this);
    }

    [RelayCommand]
    private async Task GotoAddWeightingPageAsync()
    {
        await windowService.ShowDialogAsync<AddWeightingTareWeightingViewModel>(_weightingId);

        await LoadAsync(_weightingId);
    }

    partial void OnWeightGrossChanged(double value)
    {
        if (_weightTare == 0)
            return;

        WeightNet = WeightingNetCalculator.CalculateWeightNet(value, WeightTare);
    }

    private async Task LoadAsync(int weightingId)
    {
        if (weightingId <= 0)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                "Error initializing page", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        _weightingId = weightingId;

        var getWeightingResult = await mediator.Send(new GetWeightingQuery(weightingId));

        if (getWeightingResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error getting weighting, {getWeightingResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        var getCarSelectListResult = await mediator.Send(new GetCarSelectListQuery());

        if (getCarSelectListResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error getting car list, {getCarSelectListResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        CarSelectList = new ObservableCollection<CarSelectListModel>(getCarSelectListResult.Value);
        SelectedCar = CarSelectListModel.Create(getWeightingResult.Value.Car);
        WeightGross = getWeightingResult.Value.WeightingGross.WeightKg;
        WeightTare = getWeightingResult.Value.WeightingTare?.WeightKg ?? 0;
        WeightNet = getWeightingResult.Value.WeightNetKg ?? 0;
    }
}
