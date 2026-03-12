using System;
using System.Reactive;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class MenuScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public string Title => "MenuScreenViewModel";

    public string Message => "Press \"Next\" to add another \"MainMenuViewModel\" to the ReactUI NavigationStack";

    public IScreen HostScreen { get; set; }
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToLibraryCommand { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToSettingsCommand { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> NavigateToReaderCommand { get; } // Temporary, remove after reader screen works.

    public MenuScreenViewModel(IScreen screen)
    {
        HostScreen = screen;

        NavigateToLibraryCommand = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new LibraryScreenViewModel())
        );
        
        NavigateToSettingsCommand = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new SettingsScreenViewModel())
        );

        NavigateToReaderCommand = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new ReaderScreenViewModel())
        );
    }
}