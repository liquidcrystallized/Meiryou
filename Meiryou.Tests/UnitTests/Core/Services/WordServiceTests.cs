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
        await _service.SaveWordAsync("リンゴ");
        await _service.SaveWordAsync("苺");
        await _service.SaveWordAsync("桃");

        var result = await _service.GetWordsByTextAsync(["リンゴ", "桃"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Has.Count.EqualTo(2));
        Assert.That(resultWords.Select(w => w.Text), Does.Contain("リンゴ"));
        Assert.That(resultWords.Select(w => w.Text), Does.Contain("桃"));
    }
    
    [Test]
    public async Task GetWordsByTextAsync_ShouldReturnEmpty_WhenNoMatches()
    {
        await _service.SaveWordAsync("空");

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
        await _service.SaveWordAsync("猫");

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
        var word = await _service.SaveWordAsync("犬");
        
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
        await _service.SaveWordAsync("買う");
        await _service.SaveWordAsync("買います");
        await _service.SaveWordAsync("買った");

        var result = await _service.GetWordsByTextAsync(["買う"]);
        var resultWords = result.ToList();

        Assert.That(resultWords, Has.Count.EqualTo(1));
        Assert.That(resultWords[0].Text, Is.EqualTo("買う"));
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldCreateNewWord()
    {
        var result = await _service.SaveWordAsync("新世界");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Text, Is.EqualTo("新世界"));
            Assert.That(_context.Words.Any(w => w.Id == result.Id), Is.True);
        }
    }

    [Test]
    public async Task SaveWordAsync_ShouldSetCreatedAtTimestamp()
    {
        var before = DateTime.UtcNow;
        var result = await _service.SaveWordAsync("蝶舞翠");
        var after = DateTime.UtcNow;

        Assert.That(result.CreatedAt, Is.GreaterThanOrEqualTo(before));
        Assert.That(result.CreatedAt, Is.LessThanOrEqualTo(after));
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldSetDefaultValues()
    {
        var result = await _service.SaveWordAsync("黒猫");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Definition, Is.Empty);
            Assert.That(result.PartOfSpeech, Is.Empty);
            Assert.That(result.FrequencyRank, Is.EqualTo(-1));
            Assert.That(result.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Unknown));
        }
    }

    [Test]
    public async Task SaveWordAsync_ShouldPersistToDatabase()
    {
        await _service.SaveWordAsync("虫");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_context.Words.Any(w => w.Text == "虫"), Is.True);
            Assert.That(_context.Words.Count(w => w.Text == "虫"), Is.EqualTo(1));
        }
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldCreateMultipleWords()
    {
        await _service.SaveWordAsync("買う");
        await _service.SaveWordAsync("買います");
        await _service.SaveWordAsync("買った");

        Assert.That(_context.Words.Count(w => w.Text.StartsWith("買")), Is.EqualTo(3));
    }

    [Test]
    public async Task SaveWordAsync_ShouldSaveNewWordWithSpecifiedFamiliarityLevel()
    {
        var result = await _service.SaveWordAsync("猫", WordFamiliarityLevel.WellKnown);
        
        Assert.That(result.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.WellKnown));
    }

    [Test]
    public async Task SaveWordAsync_ShouldUpdateFamiliarityLevel_WhenWordExists()
    {
        await _service.SaveWordAsync("恋", WordFamiliarityLevel.Unknown);

        var result = await _service.SaveWordAsync("恋", WordFamiliarityLevel.Known);
        
        Assert.That(result.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Known));
    }

    [Test]
    public async Task SaveWordAsync_ShouldHandleAllFamiliarityLevels()
    {
        var result1 = await _service.SaveWordAsync("一", WordFamiliarityLevel.New);
        var result2 = await _service.SaveWordAsync("二", WordFamiliarityLevel.Learning);
        var result3 = await _service.SaveWordAsync("三", WordFamiliarityLevel.Familiar);
        var result4 = await _service.SaveWordAsync("四", WordFamiliarityLevel.Known);
        var result5 = await _service.SaveWordAsync("五", WordFamiliarityLevel.WellKnown);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.New));
            Assert.That(result2.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Learning));
            Assert.That(result3.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Familiar));
            Assert.That(result4.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Known));
            Assert.That(result5.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.WellKnown));
        }
    }

    [Test]
    public async Task SaveWordAsync_ShouldReturnPersistedWord()
    {
        var result = await _service.SaveWordAsync("犬");
    
        var persistedWord = await _context.Words.FindAsync(result.Id);
        
        Assert.That(persistedWord, Is.Not.Null);
        Assert.That(persistedWord.Text, Is.EqualTo("犬"));
    }

    [Test]
    public async Task SaveWordAsync_ShouldNotCreateDuplicates_WhenSavingSameWordMultipleTimes()
    {
        await _service.SaveWordAsync("太陽");
        await _service.SaveWordAsync("太陽");
        await _service.SaveWordAsync("太陽");
    
        Assert.That(_context.Words.Count(w => w.Text == "太陽"), Is.EqualTo(1));
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldUpdateUpdatedAtTimestamp_WhenWordExists()
    {
        var word = await _service.SaveWordAsync("年月");
        var originalUpdatedAt = word.UpdatedAt;
    
        await Task.Delay(10);
    
        var updatedWord = await _service.SaveWordAsync("年月", WordFamiliarityLevel.Known);
    
        Assert.That(updatedWord.UpdatedAt, Is.GreaterThan(originalUpdatedAt));
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldHandleEmptyText()
    {
        var result = await _service.SaveWordAsync("");
    
        Assert.That(result.Text, Is.EqualTo(""));
        Assert.That(_context.Words.Any(w => w.Text == ""), Is.True);
    }
    
    [Test]
    public async Task SaveWordAsync_ShouldPreserveExistingProperties_WhenUpdating()
    {
        await _service.SaveWordAsync("未来", WordFamiliarityLevel.Unknown);
        var originalWord = await _context.Words.FirstAsync(w => w.Text == "未来");
    
        var updatedWord = await _service.SaveWordAsync("未来", WordFamiliarityLevel.Known);
    
        Assert.That(updatedWord.Id, Is.EqualTo(originalWord.Id));
        Assert.That(updatedWord.CreatedAt, Is.EqualTo(originalWord.CreatedAt));
    }
}