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

    //TODO: Content string splitting to add as individual words.
    public async Task<ReadingContent> AddContentAsync(string title, string content)
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

    public async Task<IEnumerable<Word>> GetWordsInContentAsync(int contentId)
    {
        return await _context.ReadingContentWords
            .Where(rcw => rcw.ReadingContentId == contentId)
            .Include(rcw => rcw.Word)
            .Select(rcw => rcw.Word)
            .Distinct()
            .ToListAsync();
    }

    public Task<IEnumerable<SentenceContext>> GetSentenceContextsAsync(int wordId)
    {
        throw new NotImplementedException();
    }

    public async Task<Word?> GetOrCreateWordAsync(string text)
    {
        var word = await _context.Words.FirstOrDefaultAsync(w => w.Text == text);

        if (word == null)
        {
            word = new Word
            {
                Text = text,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Words.Add(word);
            await _context.SaveChangesAsync();
        }

        return word;
    }
}