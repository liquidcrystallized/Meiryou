using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.Core.ServiceTests;

//TODO: Some tests are currently commented out as functionality "works" and passes but technically not complete.
[TestFixture]
public class ReadingContentServiceTests
{
    private MeiryouDbContext _context;
    private ReadingContentService _service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _service = new ReadingContentService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllContentsAsync_ShouldReturnEmptyList_WhenNoContentsExist()
    {
        var result = await _service.GetAllContentsAsync();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllContentsAsync_ShouldReturnAllContents()
    {
        await _service.AddContentAsync("Content 1", "Text 1");
        await _service.AddContentAsync("Content 2", "Text 2");

        var result = await _service.GetAllContentsAsync();
        var readingContents = result.ToList();
        
        Assert.That(readingContents, Is.Not.Empty);
        Assert.That(readingContents, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAllContentsAsync_ShouldOrderDescendingByCreatedAt()
    {
        await _service.AddContentAsync("Old Content", "Text");
        await Task.Delay(10);
        await _service.AddContentAsync("New Content", "Text");

        var result = await _service.GetAllContentsAsync();
        var readingContents = result.ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(readingContents[0].Title, Is.EqualTo("New Content"));
            Assert.That(readingContents[1].Title, Is.EqualTo("Old Content"));
        }
    }

    [Test]
    public async Task GetContentByIdAsync_ShouldReturnContent_WhenFound()
    {
        var content = await _service.AddContentAsync("Test Content", "Test Text");

        var result = await _service.GetContentByIdAsync(content.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Test Content"));
        Assert.That(result.Content, Is.EqualTo("Test Text"));
    }

    [Test]
    public async Task GetContentByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var result = await _service.GetContentByIdAsync(420);

        Assert.That(result, Is.Null);
    }

    //[Test]
    //public async Task GetContentByIdAsync_ShouldIncludeRelatedData()
    //{
    //    var content = await _service.AddContentAsync("Test Content", "Test Text");
    //    var word = await _service.GetOrCreateWordAsync("test");
    //    
    //    // Manually add a relationship for testing Include
    //    _context.ReadingContentWords.Add(new ReadingContentWord
    //    {
    //        ReadingContentId = content.Id,
    //        WordId = word.Id,
    //        OccurenceCount = 1
    //    });
    //    await _context.SaveChangesAsync();

    //    var result = await _service.GetContentByIdAsync(content.Id);

    //    Assert.That(result.ReadingContentWords, Is.Not.Empty);
    //    Assert.That(result.ReadingContentWords.First().Word, Is.Not.Null);
    //}

    [Test]
    public async Task AddContentAsync_ShouldAddContent_AndReturnEntityWithId()
    {
        var result = await _service.AddContentAsync("New Content", "New Text");

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.ReadingContents.Any(c => c.Id == result.Id), Is.True);
    }

    [Test]
    public async Task AddContentAsync_ShouldSetTimestamps()
    {
        var beforeAdd = DateTime.UtcNow;
        var result = await _service.AddContentAsync("Timestamp Test", "Text");
        var afterAdd = DateTime.UtcNow;

        Assert.That(result.CreatedAt, Is.GreaterThanOrEqualTo(beforeAdd));
        Assert.That(result.CreatedAt, Is.LessThanOrEqualTo(afterAdd));
        Assert.That(result.UpdatedAt, Is.GreaterThanOrEqualTo(beforeAdd));
        Assert.That(result.UpdatedAt, Is.LessThanOrEqualTo(afterAdd));
    }

    [Test]
    public async Task AddContentAsync_ShouldHandleEmptyContent()
    {
        await _service.AddContentAsync("Empty Content", "");
        var result = await _service.GetAllContentsAsync();
        var readingContents = result.ToList();

        Assert.That(readingContents, Is.Not.Empty);
    }

    [Test]
    public async Task AddContentAsync_ShouldHandleEmptyTitle()
    {
       await _service.AddContentAsync("", "wow");
       var result = await _service.GetAllContentsAsync();
       var readingContents = result.ToList();

       Assert.That(readingContents, Is.Not.Empty);
       Assert.That(readingContents.First().Title, Is.EqualTo("untitled"));
    }

    [Test]
    public async Task DeleteContentAsync_ShouldDeleteExistingContent()
    {
        var content = await _service.AddContentAsync("To Delete", "Text");

        await _service.DeleteContentAsync(content.Id);

        Assert.That(_context.ReadingContents.Any(c => c.Id == content.Id), Is.False);
    }

    [Test]
    public async Task DeleteContentAsync_ShouldDoNothing_WhenNotFound()
    {
        await _service.AddContentAsync("Keep This", "Text");

        await _service.DeleteContentAsync(999); // Non-existent ID

        Assert.That(_context.ReadingContents.Any(), Is.True);
    }

    [Test]
    public async Task DeleteContentAsync_ShouldCascadeDeleteRelationships()
    {
        var content = await _service.AddContentAsync("Cascade Test", "Text");
        var word = await _service.GetOrCreateWordAsync("cascade");
        
        _context.ReadingContentWords.Add(new ReadingContentWord
        {
            ReadingContentId = content.Id,
            WordId = word.Id
        });
        await _context.SaveChangesAsync();

        await _service.DeleteContentAsync(content.Id);

        Assert.That(_context.ReadingContentWords.Any(rcw => rcw.ReadingContentId == content.Id), Is.False);
    }

    [Test]
    public async Task GetWordsInContentAsync_ShouldReturnWords_WhenAssociated()
    {
        var content = await _service.AddContentAsync("Words Test", "Text");
        var word1 = await _service.GetOrCreateWordAsync("word1");
        var word2 = await _service.GetOrCreateWordAsync("word2");

        _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word1.Id });
        _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word2.Id });
        await _context.SaveChangesAsync();

        var result = await _service.GetWordsInContentAsync(content.Id);
        var listOfWords = result.ToList();
        
        Assert.That(listOfWords, Has.Count.EqualTo(2));
        Assert.That(listOfWords.Select(w => w.Id), Does.Contain(word1.Id));
        Assert.That(listOfWords.Select(w => w.Id), Does.Contain(word2.Id));
    }

    [Test]
    public async Task GetWordsInContentAsync_ShouldReturnEmpty_WhenNoWordsAssociated()
    {
        var content = await _service.AddContentAsync("Empty Words", "Text");

        var result = await _service.GetWordsInContentAsync(content.Id);

        Assert.That(result, Is.Empty);
    }

    //[Test]
    //public async Task GetWordsInContentAsync_ShouldReturnDistinctWords()
    //{
    //    var content = await _service.AddContentAsync("Distinct Test", "Text");
    //    var word = await _service.GetOrCreateWordAsync("common");

    //    // Add same word twice
    //    _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word.Id });
    //    _context.ReadingContentWords.Add(new ReadingContentWord { ReadingContentId = content.Id, WordId = word.Id });
    //    await _context.SaveChangesAsync();

    //    var result = await _service.GetWordsInContentAsync(content.Id);
    //    var readingContents = result.ToList();

    //    Assert.That(readingContents, Has.Count.EqualTo(1));
    //    Assert.That(readingContents.First().Id, Is.EqualTo(word.Id));
    //}

    //[Test]
    //public async Task GetSentenceContextsAsync_ShouldReturnContexts_WhenExists()
    //{
    //    // Arrange
    //    var word = await _service.GetOrCreateWordAsync("context");
    //    _context.SentenceContexts.Add(new SentenceContext
    //    {
    //        WordId = word.Id,
    //        Sentence = "This is a test sentence.",
    //        ReadingContentId = 1
    //    });
    //    await _context.SaveChangesAsync();

    //    // Act
    //    var result = await _service.GetSentenceContextsAsync(word.Id);

    //    // Assert
    //    Assert.That(result, Is.Not.Empty);
    //    Assert.That(result.First().Sentence, Is.EqualTo("This is a test sentence."));
    //}

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
}