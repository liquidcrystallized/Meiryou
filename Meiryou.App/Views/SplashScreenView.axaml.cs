using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Meiryou.ViewModels;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class SplashScreenView : ReactiveUserControl<SplashScreenViewModel>
{
    public SplashScreenView()
    {
        InitializeComponent();
    }
}