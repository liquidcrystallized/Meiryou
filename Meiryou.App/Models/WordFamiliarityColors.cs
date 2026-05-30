using Avalonia.Media;
using Meiryou.Core.Models;

namespace Meiryou.Models;

/**
 * Defaults are a modified Catppuccin Mocha.
 * //TODO Make this changeable by user.
 */
public static class WordFamiliarityColors
{
    public static SolidColorBrush GetBackgroundColor(WordFamiliarityLevel level)
    {
        return level switch
        {
            WordFamiliarityLevel.Unknown => new SolidColorBrush(Color.Parse("#9199cc")),       // Lavender
            WordFamiliarityLevel.New => new SolidColorBrush(Color.Parse("#b3677c")),           // Red
            WordFamiliarityLevel.Learning => new SolidColorBrush(Color.Parse("#b4a47f")),      // Yellow
            WordFamiliarityLevel.Familiar => new SolidColorBrush(Color.Parse("#b38060")),      // Peach
            WordFamiliarityLevel.Known => new SolidColorBrush(Color.Parse("#83b37f")),         // Green
            WordFamiliarityLevel.WellKnown => SolidColorBrush.Parse("Transparent"),            // No color
            _ => new SolidColorBrush(Color.Parse("#9199cc"))                                   // Default Grayish Blue
        };
    }
    
    public static SolidColorBrush GetForegroundColor(WordFamiliarityLevel level)
    {
        return level switch
        {
            _ => new SolidColorBrush(Color.Parse("#CDD6F4"))
            //WordFamiliarityLevel.Unknown => new SolidColorBrush(Color.Parse("#11111b")),       
            //WordFamiliarityLevel.New => new SolidColorBrush(Color.Parse("#11111b")),           
            //WordFamiliarityLevel.Learning => new SolidColorBrush(Color.Parse("#11111b")),      
            //WordFamiliarityLevel.Familiar => new SolidColorBrush(Color.Parse("#11111b")),      
            //WordFamiliarityLevel.Known => new SolidColorBrush(Color.Parse("#11111b")),         
            //WordFamiliarityLevel.WellKnown => new SolidColorBrush(Color.Parse("#CDD6F4")),     
            //_ => new SolidColorBrush(Color.Parse("#CDD6F4"))
        };
    }
}