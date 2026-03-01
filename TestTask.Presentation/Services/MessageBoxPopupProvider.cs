using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using TestTaskOne.Contracts;
using TestTaskOne.ViewModels;

namespace TestTaskOne.Services;

/// <inheritdoc />
internal sealed class MessageBoxPopupProvider(IWindowService windowService) : IMessageBoxPopupProvider
{
    public Task<ButtonResult> ShowMessageBoxPopup(IMsBox<ButtonResult> msgBox, ViewModelBase? caller = null)
    {
        Window? owner = null;

        if (caller != null)
            owner = windowService.GetWindowForViewModel(caller);

        owner ??= windowService.GetActiveWindow();

        return owner == null
            ? throw new InvalidOperationException("Cannot determine owner window for MessageBox.")
            : msgBox.ShowAsPopupAsync(owner);
    }
}
