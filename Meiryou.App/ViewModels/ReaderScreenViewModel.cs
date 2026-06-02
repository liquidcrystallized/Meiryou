using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Meiryou.Core.Models;
using ReactiveUI;

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
    private readonly Dictionary<string, Word> _mockDictionary = new(StringComparer.OrdinalIgnoreCase)
    {
        { "この", new Word { Definition = "this (something or someone close to the speaker (including the speaker), or ideas expressed by the speaker)", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "曲", new Word { Definition = "composition; piece of music; song; track (on a record)", PartOfSpeech = "Noun", FrequencyRank = 5325, FamiliarityLevel = GenerateRandomLevel() } },
        { "が", new Word { Definition = "A particle which indicated the subject", PartOfSpeech = "?", FrequencyRank = 5, FamiliarityLevel = GenerateRandomLevel() } },
        { "やさしい", new Word { Definition = "Multiple definitions not implemented", PartOfSpeech = "I-adjective", FrequencyRank = 521, FamiliarityLevel =  GenerateRandomLevel() } },
        { "って言う", new Word { Definition = "Multiple definitions not implemented", PartOfSpeech = "?", FrequencyRank = 234, FamiliarityLevel = GenerateRandomLevel() } },
        { "ん", new Word { Definition = "Multiple definitions not implemented", PartOfSpeech = "?", FrequencyRank = 42, FamiliarityLevel = GenerateRandomLevel() } },
        { "です", new Word { Definition = "Multiple definitions not implemented", PartOfSpeech = "?", FrequencyRank = 251, FamiliarityLevel = GenerateRandomLevel() } },
        { "か", new Word { Definition = "Multiple definitions not implemented", PartOfSpeech = "?", FrequencyRank = 4, FamiliarityLevel = GenerateRandomLevel() } },
        { "何も", new Word { Definition = "Definition", PartOfSpeech = "PartOfSpeech", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "分かっていない", new Word { Definition = "Definition", PartOfSpeech = "PartOfSpeech", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "から", new Word { Definition = "Definition", PartOfSpeech = "PartOfSpeech", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "そんな", new Word { Definition = "Definition", PartOfSpeech = "PartOfSpeech", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "こと", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "を", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "言う", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "よ", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "弾ける", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "もの", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "なら", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } },
        { "弾いてみなさい", new Word { Definition = "Definition", PartOfSpeech = "?", FrequencyRank = 1, FamiliarityLevel = GenerateRandomLevel() } }
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
        string[] words = { "この", "曲", "が", "やさしい", "って言う", "ん", "です", "か", "？", "何も", "分かっていない", "から", "そんな", "こと", "を", "言う", "ん", "です", "よ", "。", "弾ける", "もの", "なら", "弾いてみなさい" };

    foreach (var word in words)
        {
            var stats = _mockDictionary.TryGetValue(word.ToLower(), out var s)
                ? s
                : new Word { Text = word, Definition = "No definition available", PartOfSpeech = "Unknown", FrequencyRank = -1, FamiliarityLevel =  GenerateRandomLevel() };
            
            Words.Add(new WordEntry
            {
                Word = new Word
                {
                    Text = word,
                    Definition = stats.Definition,
                    PartOfSpeech = stats.PartOfSpeech,
                    FrequencyRank = stats.FrequencyRank,
                    FamiliarityLevel =  stats.FamiliarityLevel,
                },
            });

            //if (words.Last() != word)
            //{
            //    Words.Add(new WordEntry
            //    {
            //        Word = new Word { Text = " ", FamiliarityLevel = WordFamiliarityLevel.WellKnown},
            //        //BackgroundBrush = new SolidColorBrush(backgroundColour),
            //        //ForegroundBrush = new SolidColorBrush(foregroundColour),
            //        IsSpace = true,
            //    });
            //}
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