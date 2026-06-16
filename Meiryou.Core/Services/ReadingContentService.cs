using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Core.Services;

public class ReadingContentService : IReadingContentService
{
    private readonly MeiryouDbContext _context;

    public ReadingContentService(MeiryouDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ReadingContent>> GetAllContentsAsync()
    {
        return await _context.ReadingContents
            .OrderByDescending(rc => rc.CreatedAt)
            .ToListAsync();
    }

    public async Task<ReadingContent?> GetContentByIdAsync(int id)
    {
        return await _context.ReadingContents
            .Include(rc => rc.ReadingContentWords)
            .ThenInclude(rcw => rcw.Word)
            .FirstOrDefaultAsync(rc => rc.Id == id);
    }

    public async Task<ReadingContent> ImportContentAsync(string title, string content)
    {
        var readingContent = new ReadingContent
        {
            Title = string.IsNullOrWhiteSpace(title) ? "untitled" : title,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.ReadingContents.Add(readingContent);
        await _context.SaveChangesAsync();

        return readingContent;
    }

    public async Task DeleteContentAsync(int id)
    {
        var content = await _context.ReadingContents.FindAsync(id);
        
        if (content != null)
        {
            _context.ReadingContents.Remove(content);
            await _context.SaveChangesAsync();
        }
    }
}