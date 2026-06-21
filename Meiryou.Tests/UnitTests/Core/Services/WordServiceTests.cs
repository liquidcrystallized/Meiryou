using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.UnitTests.Core.Services;

[TestFixture]
public class WordServiceTests
{
    private MeiryouDbContext _context;
    private WordService _service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _service = new WordService(_context);
    }
    
    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [Test]
    public async Task GetOrCreateWordAsync_ShouldReturnExistingWord()
    {
        var existingWord = await _service.GetOrCreateWordAsync("existing");
        var originalId = existingWord.Id;
        var originalCreatedAt = existingWord.CreatedAt;

        var result = await _service.GetOrCreateWordAsync("existing");

        Assert.That(result.Id, Is.EqualTo(originalId));
        Assert.That(result.CreatedAt, Is.EqualTo(originalCreatedAt));
    }

    [Test]
    public async Task GetOrCreateWordAsync_ShouldCreateNewWord_WhenNotExists()
    {
        var result = await _service.GetOrCreateWordAsync("new-word");

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Text, Is.EqualTo("new-word"));
        Assert.That(_context.Words.Any(w => w.Id == result.Id), Is.True);
    }

    [Test]
    public async Task GetOrCreateWordAsync_ShouldSetTimestamps_OnNewWord()
    {
        var before = DateTime.UtcNow;
        var result = await _service.GetOrCreateWordAsync("timestamp-test");
        var after = DateTime.UtcNow;

        Assert.That(result.CreatedAt, Is.GreaterThanOrEqualTo(before));
        Assert.That(result.CreatedAt, Is.LessThanOrEqualTo(after));
    }

    [Test]
    public async Task GetOrCreateWordAsync_ShouldReturnWordWithDefaults_OnNewWord()
    {
        var result = await _service.GetOrCreateWordAsync("食べる");
    
        Assert.That(result.Definition, Is.Empty);
        Assert.That(result.PartOfSpeech, Is.Empty);
        Assert.That(result.FrequencyRank, Is.EqualTo(-1));
        Assert.That(result.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Unknown));
    }
}