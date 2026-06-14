using System;
using System.Reactive;
using Meiryou.Core.Services;
using Meiryou.Services;
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
            () => Router.Navigate.Execute(new LibraryScreenViewModel(
                this,
                Locator.Current.GetService<IFilesService>() ?? throw new InvalidOperationException(),
                Locator.Current.GetService<IReadingContentService>() ?? throw new InvalidOperationException(),
                Locator.Current.GetService<ITextImportService>() ?? throw new InvalidOperationException()))
        );
    }
}