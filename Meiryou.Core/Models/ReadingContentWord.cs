namespace Meiryou.Core.Models;

public class ReadingContentWord
{
    public int ReadingContentId { get; set; }
    public int WordId { get; set; }
    public int OccurenceCount { get; set; } = 1;
    
    // EF Core navigation properties.
    public ReadingContent ReadingContent { get; set; } = null!;
    public Word Word { get; set; } = null!;
}