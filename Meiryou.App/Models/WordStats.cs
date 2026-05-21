namespace Meiryou.Models;

public class WordStats
{
    public string Definition { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;

    /// <summary>
    /// How frequently this word appears in all the user's saved content.
    /// The lower the number, the more "frequent" it is.
    /// </summary>
    public int FrequencyRank { get; set; } = -1;

    public WordFamiliarityLevel WordFamiliarityLevel { get; set; } = WordFamiliarityLevel.Unknown;
}