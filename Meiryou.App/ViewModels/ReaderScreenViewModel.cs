using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Media;
using Meiryou.Models;
using ReactiveUI;
using Splat.ApplicationPerformanceMonitoring;

namespace Meiryou.ViewModels;

public class ReaderScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public string Title => "ReaderScreenViewModel";
    public IScreen HostScreen { get; set; }
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ObservableCollection<WordEntry> Words { get; } = [];

    public WordEntry? SelectedWord
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool IsPopupVisible
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveCommand<Unit, Unit> AddRandomTextCommand { get; }
    public ReactiveCommand<Unit, Unit> ClosePopupCommand { get; }
    public ReactiveCommand<WordEntry, Unit> SelectedWordCommand { get; }
    
    //TODO: Just for testing.
    private readonly Dictionary<string, WordStats> _mockDictionary = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Hello", new WordStats { Definition = "A greeting", PartOfSpeech = "Interjection", FrequencyRank = 5, WordFamiliarityLevel = GenerateRandomLevel() } },
        { "World", new WordStats { Definition = "The earth or humanity", PartOfSpeech = "Noun", FrequencyRank = 10, WordFamiliarityLevel = GenerateRandomLevel() } },
        { "This", new WordStats { Definition = "Used to indicate a specific thing", PartOfSpeech = "Pronoun", FrequencyRank = 3, WordFamiliarityLevel = GenerateRandomLevel() } },
        { "is", new WordStats { Definition = "Third person singular of 'be'", PartOfSpeech = "Verb", FrequencyRank = 2, WordFamiliarityLevel =  GenerateRandomLevel() } },
        { "a", new WordStats { Definition = "One; any; an indefinite amount", PartOfSpeech = "Article", FrequencyRank = 1, WordFamiliarityLevel =  GenerateRandomLevel() } },
        { "test", new WordStats { Definition = "A sample or trial examination", PartOfSpeech = "Noun", FrequencyRank = 450, WordFamiliarityLevel =  GenerateRandomLevel() } },
    };

    public ReaderScreenViewModel()
    {
        AddRandomTextCommand = ReactiveCommand.Create(AddRandomText);
        ClosePopupCommand = ReactiveCommand.Create(ClosePopup);
        SelectedWordCommand = ReactiveCommand.Create<WordEntry>(SelectWord);
        
        //TODO: Remove later, just some content for immediate visualisation.
        AddRandomText();
    }

    //TODO: Temporary, remove/change later.
    private void AddRandomText()
    {
        string[] words = { "Hello", "World", "This", "is", "a", "test" };

        foreach (var word in words)
        {
            var stats = _mockDictionary.TryGetValue(word.ToLower(), out var s)
                ? s
                : new WordStats { Definition = "No definition available", PartOfSpeech = "Unknown", FrequencyRank = -1, WordFamiliarityLevel =  GenerateRandomLevel() };

            var backgroundColour = WordFamiliarityColors.GetBackgroundColor(stats.WordFamiliarityLevel).Color;
            var foregroundColour = WordFamiliarityColors.GetForegroundColor(stats.WordFamiliarityLevel).Color;
            
            Words.Add(new WordEntry
            {
                Data = new WordData { Text = word },
                BackgroundBrush = new SolidColorBrush(backgroundColour),
                ForegroundBrush = new SolidColorBrush(foregroundColour),
                Stats = stats
            });

            if (words.Last() != word)
            {
                Words.Add(new WordEntry
                {
                    Data = new WordData { Text = " " },
                    BackgroundBrush = new SolidColorBrush(backgroundColour),
                    ForegroundBrush = new SolidColorBrush(foregroundColour),
                    IsSpace = true,
                    Stats = stats
                });
            }
        }
    }

    //TODO: Only for testing.
    private static WordFamiliarityLevel GenerateRandomLevel()
    {
        var random = new Random();
        var levels = Enum.GetValues<WordFamiliarityLevel>();
        var randomIndex = random.Next(levels.Length);

        return levels[randomIndex];
    }

    private void ClosePopup()
    {
        IsPopupVisible = false;
        SelectedWord = null;
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