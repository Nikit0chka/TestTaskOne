using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TestTask.Application.Cars.Dto;
using TestTask.Application.Cars.Queries.GetSelectList;
using TestTask.Application.Weightings.UseCases.Create;
using TestTaskOne.Contracts;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class CreateWeightingViewModel(
    IMediator mediator,
    IWindowService windowService,
    IMessageBoxPopupProvider messageBoxPopupProvider) : ViewModelBase, IAsyncInitializableViewModel
{
    [ObservableProperty] private ObservableCollection<CarSelectListModel> _carSelectList = [];
    [ObservableProperty] private CarSelectListModel _selectedCar;
    [ObservableProperty] private double _weightGross;

    public async Task InitializeAsync(object? parameter)
    {
        var getCarSelectListResult = await mediator.Send(new GetCarSelectListQuery());

        if (getCarSelectListResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error getting car list, {getCarSelectListResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        CarSelectList = new ObservableCollection<CarSelectListModel>(getCarSelectListResult.Value);
    }

    [RelayCommand]
    private async Task CreateAsync()
    {
        var createWeightingResult = await mediator.Send(new CreateWeightingCommand(SelectedCar.Id, WeightGross));

        if (createWeightingResult.IsError)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard("Error",
                $"Error creating weighting, {createWeightingResult.FirstError.Description}", ButtonEnum.Ok, Icon.Error);

            await messageBoxPopupProvider.ShowMessageBoxPopup(messageBox);
            return;
        }

        windowService.CloseWindow(this);
    }
}
