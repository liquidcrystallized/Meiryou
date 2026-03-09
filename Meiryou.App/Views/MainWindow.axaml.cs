using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}