using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Meiryou.Tests.UnitTests.Core.Services;

public class TextImportServiceTests
{
    private MeiryouDbContext _context;
    private TextImportService _service;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<MeiryouDbContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
        
        _context = new MeiryouDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _service = new TextImportService(_context);
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

        await _service.ImportTextAsync(title, content);
        var readingContent = await _context.ReadingContents.FirstOrDefaultAsync(rc => rc.Title == title);
        
        Assert.That(readingContent, Is.Not.Null);
        Assert.That(readingContent.Content, Is.EqualTo(content));
    }
}