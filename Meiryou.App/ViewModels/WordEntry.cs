using Avalonia.Media;
using Meiryou.Models;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class WordEntry : ReactiveObject
{
    public WordData Data { get; set; } = new();
    public WordStats Stats { get; set; } = new();

    public SolidColorBrush? ForegroundBrush
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool IsSelected
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    
    public bool IsSpace { get; set; }
}