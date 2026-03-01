using CommunityToolkit.Mvvm.ComponentModel;

namespace TestTaskOne.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentViewModel;
}
