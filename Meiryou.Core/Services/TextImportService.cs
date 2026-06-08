using Meiryou.Core.Data;
using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

public class TextImportService : ITextImportService
{
    private readonly MeiryouDbContext _context;

    public TextImportService(MeiryouDbContext context)
    {
        _context = context;
    }
    
    public async Task ImportTextAsync(string title, string content)
    {
        var readingContent = new ReadingContent
        {
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ReadingContents.Add(readingContent);
        await _context.SaveChangesAsync();
    }
}