using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reactive;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Meiryou.Services;
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

    private readonly IFilesService _filesService;
    private readonly IReadingContentService _readingContentService;
    private readonly ITextImportService _textImportService;

    public LibraryScreenViewModel(IFilesService filesService, IReadingContentService readingContentService, ITextImportService textImportService)
    {
        _filesService = filesService ?? throw new ArgumentNullException(nameof(filesService));
        _readingContentService = readingContentService ?? throw new ArgumentNullException(nameof(readingContentService));
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
}