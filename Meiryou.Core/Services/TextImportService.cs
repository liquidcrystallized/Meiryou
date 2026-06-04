using Meiryou.Core.Data;
using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

public class TextImportService : ITextImportService
{
    public async Task ImportTextAsync(string title, string content, MeiryouDbContext context)
    {
        var readingContent = new ReadingContent
        {
            Title = title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        context.ReadingContents.Add(readingContent);
        await context.SaveChangesAsync();
        
        //TODO: Japanese text parsing.
    }
}