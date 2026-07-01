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
    public async Task GetWordsByTextAsync_ShouldReturnMatchingWords()
    {
        await _service.CreateWordAsync("リンゴ");
        await _service.CreateWordAsync("苺");
        await _service.CreateWordAsync("桃");

        var result = await _service.GetWordsByTextAsync(["リンゴ", "桃"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Has.Count.EqualTo(2));
        Assert.That(resultWords.Select(w => w.Text), Does.Contain("リンゴ"));
        Assert.That(resultWords.Select(w => w.Text), Does.Contain("桃"));
    }
    
    [Test]
    public async Task GetWordsByTextAsync_ShouldReturnEmpty_WhenNoMatches()
    {
        await _service.CreateWordAsync("空");

        var result = await _service.GetWordsByTextAsync(["土", "海"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Is.Empty);
    }

    [Test]
    public async Task GetWordsByTextAsync_ShouldReturnEmpty_WhenEmptyList()
    {
        var result = await _service.GetWordsByTextAsync([]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Is.Empty);
    }
    
    [Test]
    public async Task GetWordsByTextAsync_ShouldHandleDuplicatesInInput()
    {
        await _service.CreateWordAsync("猫");

        var result = await _service.GetWordsByTextAsync(["猫", "猫", "猫"]);
        var resultWords = result.ToList();

        // Should return one entry per matching word in DB, not per duplicate in input.
        // So if we tried to get the same word multiple times, only 1 result should exist.
        Assert.That(resultWords, Has.Count.EqualTo(1));
        Assert.That(resultWords.All(w => w.Text == "猫"), Is.True);
    }

    [Test]
    public async Task GetWordsByTextAsync_ShouldReturnFullWordObjects()
    {
        var word = await _service.CreateWordAsync("犬");
        
        var result = await _service.GetWordsByTextAsync(["犬"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Has.Count.EqualTo(1));
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resultWords[0].Id, Is.EqualTo(word.Id));
            Assert.That(resultWords[0].Text, Is.EqualTo("犬"));
        }
    }

    [Test]
    public async Task GetWordsByTextAsync_ShouldReturnExactMatchesNotPartial()
    {
        await _service.CreateWordAsync("買う");
        await _service.CreateWordAsync("買います");
        await _service.CreateWordAsync("買った");

        var result = await _service.GetWordsByTextAsync(["買う"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Has.Count.EqualTo(1));
        Assert.That(resultWords[0].Text, Is.EqualTo("買う"));
    }
    
    [Test]
    public async Task CreateWordAsync_ShouldCreateNewWord()
    {
        var result = await _service.CreateWordAsync("新世界");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Text, Is.EqualTo("新世界"));
            Assert.That(_context.Words.Any(w => w.Id == result.Id), Is.True);
        }
    }

    [Test]
    public async Task CreateWordAsync_ShouldSetCreatedAtTimestamp()
    {
        var before = DateTime.UtcNow;
        var result = await _service.CreateWordAsync("蝶舞翠");
        var after = DateTime.UtcNow;

        Assert.That(result.CreatedAt, Is.GreaterThanOrEqualTo(before));
        Assert.That(result.CreatedAt, Is.LessThanOrEqualTo(after));
    }
    
    [Test]
    public async Task CreateWordAsync_ShouldSetDefaultValues()
    {
        var result = await _service.CreateWordAsync("黒猫");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Definition, Is.Empty);
            Assert.That(result.PartOfSpeech, Is.Empty);
            Assert.That(result.FrequencyRank, Is.EqualTo(-1));
            Assert.That(result.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Unknown));
        }
    }

    [Test]
    public async Task CreateWordAsync_ShouldPersistToDatabase()
    {
        await _service.CreateWordAsync("虫");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_context.Words.Any(w => w.Text == "虫"), Is.True);
            Assert.That(_context.Words.Count(w => w.Text == "虫"), Is.EqualTo(1));
        }
    }
    
    [Test]
    public async Task CreateWordAsync_ShouldCreateMultipleWords()
    {
        await _service.CreateWordAsync("買う");
        await _service.CreateWordAsync("買います");
        await _service.CreateWordAsync("買った");

        Assert.That(_context.Words.Count(w => w.Text.StartsWith("買")), Is.EqualTo(3));
    }
}