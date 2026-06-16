using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.UnitTests.Core.Services;

[TestFixture]
public class ContentWordServiceTests
{
    private MeiryouDbContext _context;
    private ContentWordService _contentWordService;
    private ReadingContentService _readingContentService;
    private WordService _wordService;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        
        _readingContentService = new ReadingContentService(_context);
        _wordService = new WordService(_context);
        _contentWordService = new ContentWordService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetWordsInContentAsync_ShouldReturnWords_WhenAssociated()
    {
        var content = await _readingContentService.ImportContentAsync("Words Test", "Text");
        var word1 = await _wordService.GetOrCreateWordAsync("word1");
        var word2 = await _wordService.GetOrCreateWordAsync("word2");

        _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word1.Id });
        _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word2.Id });
        await _context.SaveChangesAsync();

        var result = await _contentWordService.GetWordsInContentAsync(content.Id);
        var listOfWords = result.ToList();
        
        Assert.That(listOfWords, Has.Count.EqualTo(2));
        Assert.That(listOfWords.Select(w => w.Id), Does.Contain(word1.Id));
        Assert.That(listOfWords.Select(w => w.Id), Does.Contain(word2.Id));
    }

    [Test]
    public async Task GetWordsInContentAsync_ShouldReturnEmpty_WhenNoWordsAssociated()
    {
        var content = await _readingContentService.ImportContentAsync("Empty Words", "Text");

        var result = await _contentWordService.GetWordsInContentAsync(content.Id);

        Assert.That(result, Is.Empty);
    }
}