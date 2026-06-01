using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Meiryou.Core.Models;

namespace Meiryou.Converters;

public class FamiliarityToGradientBrushConverter : IValueConverter
{
    public static readonly FamiliarityToGradientBrushConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not WordFamiliarityLevel level)
            return Brushes.Transparent;

        return level switch
        {
            WordFamiliarityLevel.Unknown => CreateGradient(Colors.Blue),
            WordFamiliarityLevel.New => CreateGradient(Colors.Red),
            WordFamiliarityLevel.Learning => CreateGradient(Colors.Orange),
            WordFamiliarityLevel.Familiar => CreateGradient(Colors.Yellow),
            WordFamiliarityLevel.Known => CreateGradient(Colors.Green),
            WordFamiliarityLevel.WellKnown => CreateGradient(Colors.Transparent),
            _ => Brushes.Transparent
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }

    private LinearGradientBrush CreateGradient(Color startColor)
    {
        var endColor = Color.FromArgb(0, startColor.R, startColor.G, startColor.B);
        
        return new LinearGradientBrush
        {
            StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            EndPoint = new RelativePoint(0.5, 0.75, RelativeUnit.Relative),
            GradientStops =
            [
                new GradientStop(startColor, 0.0),
                new GradientStop(endColor, 0.75)
            ]
        };
    }
}