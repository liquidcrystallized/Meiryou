namespace Meiryou.Core.Models;

public class Word
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? Definition { get; set; }
    public string? PartOfSpeech { get; set; }
    public int FrequencyRank { get; set; } = -1;
    public WordFamiliarityLevel FamiliarityLevel { get; set; } = WordFamiliarityLevel.Unknown;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // EF Core navigation properties.
    public ICollection<ReadingContentWord> ReadingContentWords { get; set; } = new List<ReadingContentWord>();
    public ICollection<SentenceContext> SentenceContexts { get; set; } = new List<SentenceContext>();
}