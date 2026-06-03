namespace Meiryou.Core.Models;

public class SentenceContext
{
    public int Id { get; set; }
    public int WordId { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public int ReadingContentId { get; set; }
    
    // EF Core navigation properties.
    public Word Word { get; set; } = null!;
    public ReadingContent ReadingContent { get; set; } = null!;
}