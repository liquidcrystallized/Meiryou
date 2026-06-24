using Meiryou.Core.Services.TextParsing;

namespace Meiryou.Tests.UnitTests.Core.Services.TextParsing;

public abstract class TextParsingServiceSharedBehaviourTests
{
    protected ITextParsingService Service;

    [SetUp]
    public void SetUp()
    {
        Service = CreateService();
    }

    [Test]
    public void SegmentTextIntoWords_ShouldReturnEmptyList_OnEmptyInput()
    {
        var content = string.Empty;
        
        var result = Service.SegmentTextIntoWords(content);

        Assert.That(result, Is.Empty);
    }
    
    protected abstract ITextParsingService CreateService();
}