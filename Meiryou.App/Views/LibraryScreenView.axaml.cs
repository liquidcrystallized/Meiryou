using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class LibraryScreenView : ReactiveUserControl<LibraryScreenViewModel>
{
    public LibraryScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }
}