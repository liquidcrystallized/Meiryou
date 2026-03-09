using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class MenuScreenView : ReactiveUserControl<MenuScreenViewModel>
{
    public MenuScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}