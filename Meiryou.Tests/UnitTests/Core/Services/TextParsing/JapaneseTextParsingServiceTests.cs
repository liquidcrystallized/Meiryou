using Meiryou.Core.Services.TextParsing;

namespace Meiryou.Tests.UnitTests.Core.Services.TextParsing;

[TestFixture]
public class JapaneseTextParsingServiceTests
{
    private JapaneseTextParsingService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new JapaneseTextParsingService();
    }
}