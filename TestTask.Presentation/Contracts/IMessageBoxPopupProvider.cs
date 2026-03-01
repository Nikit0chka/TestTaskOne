using System.Threading.Tasks;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Enums;
using TestTaskOne.ViewModels;

namespace TestTaskOne.Contracts;

/// <summary>
///     Сервис отображения окна-диалога
/// </summary>
public interface IMessageBoxPopupProvider
{
    /// <summary>
    ///     Отобразить окно-диалог
    /// </summary>
    /// <param name="msgBox">Окно-диалог для отображения</param>
    /// <param name="caller">Источник вызова</param>
    Task<ButtonResult> ShowMessageBoxPopup(IMsBox<ButtonResult> msgBox, ViewModelBase? caller = null);
}
