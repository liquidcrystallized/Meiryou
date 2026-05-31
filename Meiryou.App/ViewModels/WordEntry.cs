using Avalonia.Media;
using Meiryou.Core.Models;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class WordEntry : ReactiveObject
{
    public Word Word { get; set; } = new();

    public SolidColorBrush? BackgroundBrush
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

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