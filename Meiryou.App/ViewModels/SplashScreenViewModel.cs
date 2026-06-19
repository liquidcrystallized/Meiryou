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

        await HostScreen.Router.Navigate.Execute(libraryViewModel);
    }
}