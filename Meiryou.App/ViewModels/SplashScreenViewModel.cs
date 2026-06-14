using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Meiryou.Core.Services;
using Meiryou.Services;
using ReactiveUI;
using Splat;

namespace Meiryou.ViewModels;

public class SplashScreenViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IFilesService _filesService;
    private readonly IReadingContentService _readingContentService;
    private readonly ITextImportService _textImportService;
    
    public IScreen HostScreen { get; set; }
    
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
        
        _filesService = Locator.Current.GetService<IFilesService>() ?? throw new InvalidOperationException("IFilesService not found");
        _readingContentService = Locator.Current.GetService<IReadingContentService>() ?? throw new InvalidOperationException("IReadingContentService not found");
        _textImportService = Locator.Current.GetService<ITextImportService>() ?? throw new InvalidOperationException("ITextImportService not found");
        
        _ = InitialiseAsync();
    }

    private async Task InitialiseAsync()
    {
        var libraryViewModel = new LibraryScreenViewModel(HostScreen, _filesService, _readingContentService, _textImportService);

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