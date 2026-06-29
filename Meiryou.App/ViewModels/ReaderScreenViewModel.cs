using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Meiryou.Core.Services.TextParsing;
using ReactiveUI;
using Splat;

namespace Meiryou.ViewModels;

public class ReaderScreenViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly ITextParsingServiceFactory _textParsingServiceFactory;
    private readonly IWordService _wordService;
    
    public IScreen HostScreen { get; }
    public ObservableCollection<WordEntry> Words { get; } = [];

    public ReadingContent? CurrentContent
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    
    public bool IsPopupVisible
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WordEntry? SelectedWord
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    
    public string Title => "ReaderScreenViewModel";
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ReactiveCommand<Unit, Unit> ClosePopupCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; }
    public ReactiveCommand<WordEntry, Unit> SelectedWordCommand { get; }

    public ReaderScreenViewModel(IScreen screen) : this(screen,
        Locator.Current.GetService<ITextParsingServiceFactory>() ?? throw new InvalidOperationException(),
        Locator.Current.GetService<IWordService>() ?? throw new InvalidOperationException())
    {
        
    }

    public ReaderScreenViewModel(IScreen screen, ITextParsingServiceFactory textParsingServiceFactory, IWordService wordService)
    {
        HostScreen = screen;
        _textParsingServiceFactory = textParsingServiceFactory;
        _wordService = wordService;
        
        ClosePopupCommand = ReactiveCommand.Create(ClosePopup);
        NavigateBackCommand = ReactiveCommand.Create(NavigateBack);
        SelectedWordCommand = ReactiveCommand.Create<WordEntry>(SelectWord);
    }

    public async Task LoadContent(ReadingContent content)
    {
        CurrentContent = content;
        Words.Clear();
        
        var textParsingService = _textParsingServiceFactory.GetService(CurrentContent.Language);
        var words = textParsingService.SegmentTextIntoWords(CurrentContent.Content);

        foreach (var wordText in words)
        {
            if (string.IsNullOrWhiteSpace(wordText))
            {
                // Korean has spaces, Chinese and Japanese don't.
                if (CurrentContent.Language == LanguageType.Korean)
                {
                    Words.Add(new WordEntry
                    {
                        Word = new Word { Text = " ", FamiliarityLevel = WordFamiliarityLevel.WellKnown },
                        IsSpace = true
                    });
                }
                else
                {
                    Words.Add(new WordEntry
                    {
                        Word = new Word { Text = "", FamiliarityLevel =  WordFamiliarityLevel.WellKnown },
                        IsSpace =  false
                    });
                }

                continue;
            }

            var word = await _wordService.GetOrCreateWordAsync(wordText);
            
            Words.Add(new WordEntry
            {
                Word = word,
                IsSpace =  false
            });
        }
    }
    
    private void ClosePopup()
    {
        IsPopupVisible = false;
        SelectedWord = null;
    }

    private void NavigateBack()
    {
        HostScreen.Router.NavigateBack.Execute();
    }

    private void SelectWord(WordEntry? word)
    {
        if (word == null || word.IsSpace) return;

        // Only toggle the popup off and on if the word being clicked is the same.
        if (IsPopupVisible && SelectedWord == word || !IsPopupVisible && SelectedWord == word)
        {
            IsPopupVisible = !IsPopupVisible;
        }
        else
        {
            IsPopupVisible = true;
        }

        SelectedWord = word;
    }
}