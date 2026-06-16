using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Core.Services;

public class ContentWordService : IContentWordService
{
    private readonly MeiryouDbContext _context;

    public ContentWordService(MeiryouDbContext context)
    {
        _context = context;
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
}