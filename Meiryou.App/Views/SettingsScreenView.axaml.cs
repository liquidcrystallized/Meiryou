using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class SettingsScreenView : ReactiveUserControl<SettingsScreenViewModel>
{
    public SettingsScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}