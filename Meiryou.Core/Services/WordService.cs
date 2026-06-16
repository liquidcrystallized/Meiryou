using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Core.Services;

public class WordService : IWordService
{
    private readonly MeiryouDbContext _context;

    public WordService(MeiryouDbContext context)
    {
        _context = context;
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