using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

    /// <summary>
    /// Prepare a piece of content to display in the reader.
    /// Basically breaking a piece of text into individual word objects
    /// and builds a display-ready collection for the view to use.
    /// </summary>
    /// <param name="content">A piece of reading content.</param>
    public async Task LoadContent(ReadingContent content)
    {
        CurrentContent = content;
        Words.Clear();
        
        var textParsingService = _textParsingServiceFactory.GetService(CurrentContent.Language);
        var wordStrings = textParsingService.SegmentTextIntoWords(CurrentContent.Content);
        var listOfWordStrings = wordStrings.ToList();
        var listOfUniqueWordStrings = listOfWordStrings
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Distinct()
            .ToList();
        
        IEnumerable<Word> existingKnownWords = await _wordService.GetWordsByTextAsync(listOfUniqueWordStrings);
        var wordLookupDictionary = existingKnownWords.ToDictionary(w => w.Text, StringComparer.Ordinal);

        foreach (var wordString in listOfWordStrings)
        {
            if (string.IsNullOrWhiteSpace(wordString))
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
                        IsSpace = false
                    });
                }
                continue;
            }

            if (wordLookupDictionary.TryGetValue(wordString, out var existingWord))
            {
                Words.Add(new WordEntry { Word = existingWord, IsSpace = false });
            }
            else
            {
                var newWord = new Word { Text = wordString };
                Words.Add(new WordEntry { Word = newWord, IsSpace = false });
            }
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