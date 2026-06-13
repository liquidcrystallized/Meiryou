using System;
using System.Reactive;
using ReactiveUI;
using Splat;

namespace Meiryou.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public RoutingState Router { get; } = new();

    public ReactiveCommand<Unit, IRoutableViewModel> NavigatePreviousCommand => Router.NavigateBack;
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateNextCommand { get; }

    public MainWindowViewModel()
    {
        NavigateNextCommand = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(Locator.Current.GetService<LibraryScreenViewModel>() ?? throw new InvalidOperationException())
        );
    }
}