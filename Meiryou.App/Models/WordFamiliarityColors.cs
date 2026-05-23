using Avalonia.Media;

namespace Meiryou.Models;

/**
 * TODO: Rethink these colors OR just let the user set them.
 */
public static class WordFamiliarityColors
{
    public static SolidColorBrush GetBackgroundColor(WordFamiliarityLevel level)
    {
        return level switch
        {
            WordFamiliarityLevel.Unknown => new SolidColorBrush(Color.Parse("#4B90B8")),       // Grayish Blue
            WordFamiliarityLevel.New => new SolidColorBrush(Color.Parse("#E32424")),           // Red
            WordFamiliarityLevel.Learning => new SolidColorBrush(Color.Parse("#FFFF00")),      // Yellow
            WordFamiliarityLevel.Familiar => new SolidColorBrush(Color.Parse("#FFA500")),      // Orange
            WordFamiliarityLevel.Known => new SolidColorBrush(Color.Parse("#90EE90")),         // Light Green
            WordFamiliarityLevel.WellKnown => SolidColorBrush.Parse("Transparent"),            // No color
            _ => new SolidColorBrush(Color.Parse("#4B90B8"))                                   // Default Grayish Blue
        };
    }
    
    public static SolidColorBrush GetForegroundColor(WordFamiliarityLevel level)
    {
        return level switch
        {
            WordFamiliarityLevel.Unknown => new SolidColorBrush(Color.Parse("#000000")),       // Black text on grayish blue
            WordFamiliarityLevel.New => new SolidColorBrush(Color.Parse("#000000")),           // Black text on yellow
            WordFamiliarityLevel.Learning => new SolidColorBrush(Color.Parse("#000000")),      // Black text on orange
            WordFamiliarityLevel.Familiar => new SolidColorBrush(Color.Parse("#006400")),      // Dark green text
            WordFamiliarityLevel.Known => new SolidColorBrush(Color.Parse("#000000")),         // Black text
            WordFamiliarityLevel.WellKnown => new SolidColorBrush(Color.Parse("#000000")),     // Black text
            _ => new SolidColorBrush(Color.Parse("#000000"))                                   // Default Black
        };
    }
}