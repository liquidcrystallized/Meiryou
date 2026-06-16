using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.UnitTests.Core.Services;

[TestFixture]
public class ReadingContentServiceTests
{
    private MeiryouDbContext _context;
    private ReadingContentService _service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
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
        await _service.ImportContentAsync("Content 1", "Text 1");
        await _service.ImportContentAsync("Content 2", "Text 2");

        var result = await _service.GetAllContentsAsync();
        var readingContents = result.ToList();
        
        Assert.That(readingContents, Is.Not.Empty);
        Assert.That(readingContents, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAllContentsAsync_ShouldOrderDescendingByCreatedAt()
    {
        await _service.ImportContentAsync("Old Content", "Text");
        await Task.Delay(10);
        await _service.ImportContentAsync("New Content", "Text");

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
        var content = await _service.ImportContentAsync("Test Content", "Test Text");

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

    [Test]
    public async Task ImportContentAsync_ShouldAddContent_AndReturnEntityWithId()
    {
        var result = await _service.ImportContentAsync("New Content", "New Text");

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(_context.ReadingContents.Any(c => c.Id == result.Id), Is.True);
    }

    [Test]
    public async Task ImportContentAsync_ShouldSetTimestamps()
    {
        var beforeAdd = DateTime.UtcNow;
        var result = await _service.ImportContentAsync("Timestamp Test", "Text");
        var afterAdd = DateTime.UtcNow;

        Assert.That(result.CreatedAt, Is.GreaterThanOrEqualTo(beforeAdd));
        Assert.That(result.CreatedAt, Is.LessThanOrEqualTo(afterAdd));
        Assert.That(result.UpdatedAt, Is.GreaterThanOrEqualTo(beforeAdd));
        Assert.That(result.UpdatedAt, Is.LessThanOrEqualTo(afterAdd));
    }

    [Test]
    public async Task ImportContentAsync_ShouldHandleEmptyContent()
    {
        await _service.ImportContentAsync("Empty Content", "");
        var result = await _service.GetAllContentsAsync();
        var readingContents = result.ToList();

        Assert.That(readingContents, Is.Not.Empty);
    }

    [Test]
    public async Task ImportContentAsync_ShouldHandleEmptyTitle()
    {
       await _service.ImportContentAsync("", "wow");
       var result = await _service.GetAllContentsAsync();
       var readingContents = result.ToList();

       Assert.That(readingContents, Is.Not.Empty);
       Assert.That(readingContents.First().Title, Is.EqualTo("untitled"));
    }

    [Test]
    public async Task DeleteContentAsync_ShouldDeleteExistingContent()
    {
        var content = await _service.ImportContentAsync("To Delete", "Text");

        await _service.DeleteContentAsync(content.Id);

        Assert.That(_context.ReadingContents.Any(c => c.Id == content.Id), Is.False);
    }

    [Test]
    public async Task DeleteContentAsync_ShouldDoNothing_WhenNotFound()
    {
        await _service.ImportContentAsync("Keep This", "Text");

        await _service.DeleteContentAsync(999); // Non-existent ID

        Assert.That(_context.ReadingContents.Any(), Is.True);
    }
}