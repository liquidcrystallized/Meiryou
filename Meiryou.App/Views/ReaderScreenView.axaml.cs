using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class ReaderScreenView : ReactiveUserControl<ReaderScreenViewModel>
{
    public ReaderScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}