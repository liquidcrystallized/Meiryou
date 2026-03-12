using Avalonia.Media;
using ReactiveUI;

namespace Meiryou.Models;

public class TextEntry : ReactiveObject
{
    public string Content
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = string.Empty;

    public SolidColorBrush? ForegroundColour
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public SolidColorBrush? BackgroundColour
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
}