using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using ReactiveUI;
using Splat;

namespace Meiryou.ViewModels;

public class LibraryScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public string Title => "LibraryScreenViewModel";

    public string Message => "Press \"Next\" to add another \"MainMenuViewModel\" to the ReactUI NavigationStack";
    
    public IScreen HostScreen { get; set; }
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

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
    
    public ObservableCollection<ReadingContent> Contents { get; } = [];

    public ReactiveCommand<Unit, Unit> LoadContentsCommand { get; }
    public ReactiveCommand<Unit, Unit> ImportTextCommand { get; }
    
    private readonly IReadingContentService _readingContentService;
    private readonly ITextImportService _textImportService;

    public LibraryScreenViewModel(IReadingContentService readingContentService, ITextImportService textImportService)
    {
        _readingContentService = readingContentService ?? throw  new ArgumentNullException(nameof(readingContentService));
        _textImportService = textImportService ?? throw new ArgumentNullException(nameof(textImportService));

        LoadContentsCommand = ReactiveCommand.CreateFromTask(LoadContentsAsync);
        ImportTextCommand = ReactiveCommand.CreateFromTask(ImportTextAsync);

        _ = LoadContentsAsync();
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

    //TODO: Maybe have an option to import a file OR a textbox where a user can copy text(?).
    private async Task ImportTextAsync()
    {
        //TODO: Show file picker or something.
        // Placeholder for now.
        IsImportingText = true;
        try
        {
            //TODO: Another placeholder, do this when the file picker exists or something.
            await Task.Delay(100);
        }
        finally
        {
            IsImportingText = false;
        }
    }
}