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
using TestTask.Application.Weightings.UseCases.AddWeightingTare;
using TestTask.Domain.Services;
using TestTaskOne.Contracts;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class AddWeightingTareWeightingViewModel(
    IMediator mediator,
    IWindowService windowService,
    IMessageBoxPopupProvider messageBoxPopupProvider)
    : ViewModelBase, IAsyncInitializableViewModel
{
    private int _weightingId;

    [ObservableProperty] private double _weightGross;
    [ObservableProperty] private double _weightTare;
    [ObservableProperty] private double _weightNet;
    [ObservableProperty] private CarSelectListModel _selectedCar;
    [ObservableProperty] private ObservableCollection<CarSelectListModel> _carSelectList = [];

    public async Task InitializeAsync(object? parameter)
    {
        if (parameter is int weightingId)
        {
            if (weightingId <= 0)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                    "Error initializing page", ButtonEnum.Ok, Icon.Error);

                await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
                return;
            }

            _weightingId = weightingId;

            var getWeightingResult =
                await mediator.Send(new GetWeightingQuery(weightingId));

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
                    $"Error getting car list, {getCarSelectListResult.FirstError.Description}", ButtonEnum.Ok,
                    Icon.Error);

                await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
                return;
            }

            CarSelectList = new ObservableCollection<CarSelectListModel>(getCarSelectListResult.Value);
            SelectedCar = CarSelectListModel.Create(getWeightingResult.Value.Car);
            WeightGross = getWeightingResult.Value.WeightingGross.WeightKg;
            WeightTare = getWeightingResult.Value.WeightingTare?.WeightKg ?? 0;
        }
        else
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                "Error initializing page", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
        }
    }

    [RelayCommand]
    private async Task SaveWeighting()
    {
        var addWeightingResult = await mediator.Send(new AddWeightingTareCommand(_weightingId, WeightTare));

        if (addWeightingResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error creating weighting, {addWeightingResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        windowService.CloseWindow(this);
    }

    partial void OnWeightTareChanged(double value)
    {
        WeightNet = WeightingNetCalculator.CalculateWeightNet(WeightGross, value);
    }
}
