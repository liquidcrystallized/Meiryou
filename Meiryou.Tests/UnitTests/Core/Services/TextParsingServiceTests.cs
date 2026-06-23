using Meiryou.Core.Services;

namespace Meiryou.Tests.UnitTests.Core.Services;

[TestFixture]
public class TextParsingServiceTests
{
    private TextParsingService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new TextParsingService();
    }
}