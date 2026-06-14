using System.Reactive;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new();

    public ReactiveCommand<Unit, IRoutableViewModel> NavigatePreviousCommand => Router.NavigateBack;
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateNextCommand { get; }

    public MainWindowViewModel()
    {
        Router.Navigate.Execute(new SplashScreenViewModel(this));
    }
}