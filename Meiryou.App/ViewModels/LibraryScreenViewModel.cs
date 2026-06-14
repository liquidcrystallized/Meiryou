using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Meiryou.Services;
using ReactiveUI;
using Splat;

namespace Meiryou.ViewModels;

public class LibraryScreenViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly IFilesService _filesService;
    private readonly IReadingContentService _readingContentService;
    private readonly ITextImportService _textImportService;
    
    public ObservableCollection<ReadingContent> Contents { get; } = [];
    public IScreen HostScreen { get; set; }

    public bool IsLoadingContents
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool IsImportingText
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
   
    public string Message => "Press \"Next\" to add another \"MainMenuViewModel\" to the ReactUI NavigationStack";
    public string Title => "LibraryScreenViewModel";
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ReactiveCommand<Unit, Unit> ImportTextCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadContentsCommand { get; }
    public ReactiveCommand<ReadingContent, Unit> SelectContentAndLoadReaderCommand { get; }

    public LibraryScreenViewModel(IScreen screen, IFilesService filesService, IReadingContentService readingContentService, ITextImportService textImportService)
    {
        HostScreen = screen;
        _filesService = filesService ?? throw new ArgumentNullException(nameof(filesService));
        _readingContentService = readingContentService ?? throw new ArgumentNullException(nameof(readingContentService));
        _textImportService = textImportService ?? throw new ArgumentNullException(nameof(textImportService));

        ImportTextCommand = ReactiveCommand.CreateFromTask(ImportTextAsync);
        LoadContentsCommand = ReactiveCommand.CreateFromTask(LoadContentsAsync);
        SelectContentAndLoadReaderCommand = ReactiveCommand.CreateFromTask<ReadingContent>(SelectContentAndLoadReaderAsync);

        _ = LoadContentsAsync();
    }
    
    //TODO: Maybe have an option for a textbox where a user can copy text(?).
    // Should probably split the file picking/text copy/import from the actual import to
    // database stuff.
    private async Task ImportTextAsync()
    {
        IsImportingText = true;
        try
        {
            var file = await _filesService.OpenFileAsync();
            if (file is null)
            {
                return;
            }
            
            await using var readStream = await file.OpenReadAsync();
            using var reader = new StreamReader(readStream);

            var contentTitle = Path.GetFileNameWithoutExtension(file.Name);
            await _textImportService.ImportTextAsync(contentTitle, await reader.ReadToEndAsync());

            _ = LoadContentsAsync();
        }
        finally
        {
            IsImportingText = false;
        }
    }

    //TODO: This could get slow if the user has 1000s of pieces of content. Chunk it up maybe?
    private async Task LoadContentsAsync()
    {
        IsLoadingContents = true;
        try
        {
            var contents = await _readingContentService.GetAllContentsAsync();
            Contents.Clear();
            foreach (var content in contents)
            {
                Contents.Add(content);
            }
        }
        finally
        {
            IsLoadingContents = false;
        }
    }

    private async Task SelectContentAndLoadReaderAsync(ReadingContent content)
    {
        var readerViewModel = Locator.Current.GetService<ReaderScreenViewModel>();
        if (readerViewModel != null)
        {
            readerViewModel.LoadContent(content);
            await HostScreen.Router.Navigate.Execute(readerViewModel);
        }
    }
}