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

    public ReactiveCommand<Unit, Unit> AddRandomTextCommand { get; }
    public ReactiveCommand<WordEntry, Unit> SelectedWordCommand { get; }
    
    //TODO: Just for testing.
    private readonly Dictionary<string, WordStats> _mockDictionary = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Hello", new WordStats { Definition = "A greeting", PartOfSpeech = "Interjection", FrequencyRank = 5 } },
        { "World", new WordStats { Definition = "The earth or humanity", PartOfSpeech = "Noun", FrequencyRank = 10 } },
        { "This", new WordStats { Definition = "Used to indicate a specific thing", PartOfSpeech = "Pronoun", FrequencyRank = 3 } },
        { "is", new WordStats { Definition = "Third person singular of 'be'", PartOfSpeech = "Verb", FrequencyRank = 2 } },
        { "a", new WordStats { Definition = "One; any; an indefinite amount", PartOfSpeech = "Article", FrequencyRank = 1 } },
        { "test", new WordStats { Definition = "A sample or trial examination", PartOfSpeech = "Noun", FrequencyRank = 450 } }
    };

    public ReaderScreenViewModel()
    {
        AddRandomTextCommand = ReactiveCommand.Create(AddRandomText);
        SelectedWordCommand = ReactiveCommand.Create<WordEntry>(SelectWordInternal);
        
        //TODO: Remove later, just some content for immediate visualisation.
        AddRandomText();
    }

    //TODO: Temporary, remove/change later.
    private void AddRandomText()
    {
        var random = new Random();
        string[] words = { "Hello", "World", "This", "is", "a", "test" };

        foreach (var word in words)
        {
            var r1 = (byte)random.Next(0, 256);
            var g1 = (byte)random.Next(0, 256);
            var b1 = (byte)random.Next(0, 256);

            var r2 = (byte)random.Next(0, 256);
            var g2 = (byte)random.Next(0, 256);
            var b2 = (byte)random.Next(0, 256);

            var foregroundColour = Color.FromRgb(r1, g1, b1);
            var backgroundColour = Color.FromRgb(r2, b2, g2);

            var stats = _mockDictionary.TryGetValue(word.ToLower(), out var s)
                ? s
                : new WordStats { Definition = "No definition available", PartOfSpeech = "Unknown", FrequencyRank = -1 };
            
            Words.Add(new WordEntry
            {
                Data = new WordData { Text = word },
                ForegroundBrush = new SolidColorBrush(foregroundColour),
                Stats = stats
            });

            if (words.Last() != word)
            {
                Words.Add(new WordEntry
                {
                    Data = new WordData { Text = " " },
                    ForegroundBrush = new SolidColorBrush(backgroundColour),
                    IsSpace = true,
                    Stats = stats
                });
            }
        }
    }

    private void SelectWordInternal(WordEntry? word)
    {
        if (word == null || word.IsSpace) return;

        SelectedWord = word;
    }
}