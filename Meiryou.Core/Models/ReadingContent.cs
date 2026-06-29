namespace Meiryou.Core.Models;

public class ReadingContent
{
    public int Id { get; set; }
    public LanguageType Language { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // EF Core navigation properties.
    public ICollection<ReadingContentWord> ReadingContentWords { get; set; } = new List<ReadingContentWord>();
}