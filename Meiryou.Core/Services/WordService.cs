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
    
    public async Task<IEnumerable<Word>> GetWordsByTextAsync(IEnumerable<string> texts)
    {
        var textList = texts.ToList();
        if (textList.Count == 0) return [];

        return await _context.Words
            .Where(w => textList.Contains(w.Text))
            .ToListAsync();
    }

    public async Task<Word> SaveWordAsync(string text, WordFamiliarityLevel familiarityLevel = WordFamiliarityLevel.Unknown)
    {
        var word = await _context.Words.FirstOrDefaultAsync(w => w.Text == text);

        if (word != null)
        {
            word.FamiliarityLevel = familiarityLevel;
            word.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            word = new Word
            {
                Text = text,
                FamiliarityLevel = familiarityLevel,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Words.Add(word);
        }
        
        await _context.SaveChangesAsync();
        
        return word;
    }
}