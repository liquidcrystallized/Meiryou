using Meiryou.Core.Services.TextParsing;

namespace Meiryou.Tests.UnitTests.Core.Services.TextParsing;

[TestFixture]
public class JapaneseTextParsingServiceTests : TextParsingServiceSharedBehaviourTests
{
    protected override ITextParsingService CreateService()
    {
        return new JapaneseTextParsingService();
    }
}