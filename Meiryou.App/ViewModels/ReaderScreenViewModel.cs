using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Media;
using Meiryou.Models;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class ReaderScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public string Title => "ReaderScreenViewModel";

    public IScreen HostScreen { get; set; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ObservableCollection<TextEntry> TextEntries { get; } = [];
    
    public ReactiveCommand<Unit, Unit> AddRandomTextCommand { get; }

    public ReaderScreenViewModel()
    {
        AddRandomTextCommand = ReactiveCommand.Create(AddRandomText);
    }

    //TODO: Temporary, remove/change later.
    private void AddRandomText()
    {
        var random = new Random();

        var r1 = (byte)random.Next(0, 256);
        var g1 = (byte)random.Next(0, 256);
        var b1 = (byte)random.Next(0, 256);
       
        var r2 = (byte)random.Next(0, 256);
        var g2 = (byte)random.Next(0, 256);
        var b2 = (byte)random.Next(0, 256);

        var foregroundColour = Color.FromRgb(r1, g1, b1);
        var backgroundColour = Color.FromRgb(r2, b2, g2);
        
        TextEntries.Add(new TextEntry
        {
            Content = $"Random Text {TextEntries.Count + 1}",
            ForegroundColour = new SolidColorBrush(foregroundColour),
            BackgroundColour = new SolidColorBrush(backgroundColour)
        });
    }
}