namespace Meiryou.Models;

public class WordStats
{
    public string Definition { get; set; } = string.Empty;
    public string PartOfSpeech { get; set; } = string.Empty;

    // -1 means word is unknown.
    public int FrequencyRank { get; set; } = -1;
}