using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TestTask.Application.Weightings.UseCases.Create;
using TestTaskOne.Contracts;

namespace TestTaskOne.ViewModels.Weighting;

public sealed partial class CreateWeightingViewModel(
    IMediator mediator,
    IWindowService windowService,
    IMessageBoxPopupProvider messageBoxPopupProvider) : ViewModelBase
{
    [ObservableProperty] private string _carNumber = "";
    [ObservableProperty] private double _weightGross;

    [RelayCommand]
    private async Task CreateAsync()
    {
        var createWeightingResult = await mediator.Send(new CreateWeightingCommand(CarNumber, WeightGross));

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
