using Meiryou.Core.Services;

namespace Meiryou.Tests.UnitTests.Core.Services;

[TestFixture]
public class TextAnalysisServiceTests
{
    private TextAnalysisService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new TextAnalysisService();
    }
}