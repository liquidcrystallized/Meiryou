using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class SplashScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    
    public bool IsLoading
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string StatusMessage
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public SplashScreenViewModel(IScreen screen)
    {
        HostScreen = screen;
        IsLoading = true;
        StatusMessage = "Loading...";
        
        _ = InitialiseAsync();
    }

    /// <summary>
    /// Preload/Initialise the library with content before routing to it. There is no way to know a user's storage
    /// device speed which means loading may be slow, so it's better to stall on the splash screen till it's done.
    /// Don't want the application to look frozen basically.
    /// </summary>
    private async Task InitialiseAsync()
    {
        var libraryViewModel = new LibraryScreenViewModel(HostScreen);

        if (libraryViewModel is null)
        {
            throw new ArgumentNullException();
        }

        await libraryViewModel.InitialiseAsync();
        
        // Let the UI thread finish loading and drawing the splash or race conditions will happen (library loads before splash).
        await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Background);
        
        // Arbitrary splash screen delay.
        await Task.Delay(2000);

        await HostScreen.Router.NavigateAndReset.Execute(libraryViewModel);
    }
}