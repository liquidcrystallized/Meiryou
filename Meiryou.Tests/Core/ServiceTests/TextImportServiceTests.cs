using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.Core.ServiceTests;

public class TextImportServiceTests
{
    private MeiryouDbContext _context;
    private TextImportService _service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _service = new TextImportService();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task ImportTextAsync_WithValidContent_CreatesReadingContent()
    {
        const string title = "Test content";
        const string content = "This is some stuff you're supposed to read";

        await _service.ImportTextAsync(title, content, _context);
        var readingContent = await _context.ReadingContents.FirstOrDefaultAsync(rc => rc.Title == title);
        
        Assert.That(readingContent, Is.Not.Null);
        Assert.That(readingContent.Content, Is.EqualTo(content));
    }
}