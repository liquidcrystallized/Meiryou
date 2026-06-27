using Meiryou.Core.Services.TextParsing;

namespace Meiryou.Tests.UnitTests.Core.Services.TextParsing;

[TestFixture]
public class JapaneseTextParsingServiceTests : TextParsingServiceSharedTests
{
    protected override ITextParsingService CreateService()
    {
        return new JapaneseTextParsingService();
    }
    
    [Test, TestCaseSource(nameof(SegmentTextIntoWords_ShouldReturnCorrectListOfWords_TestCases))]
    public void SegmentTextIntoWords_ShouldReturnCorrectListOfWords(string inputText, string[] expectedWords)
    {
        List<string> actualWords = Service.SegmentTextIntoWords(inputText).ToList();
    
        Assert.That(actualWords, Is.EqualTo(expectedWords));
    }
    private static IEnumerable<TestCaseData> SegmentTextIntoWords_ShouldReturnCorrectListOfWords_TestCases()
    {
        yield return new TestCaseData
        (
            "この曲がやさしいって言うんですか？何も分かっていないからそんなことを言うんですよ。弾けるものなら弾いてみなさい。",
            (string[]) [
                "この", "曲", "が", "やさしい", "って", "言う", "ん", "です", "か", "？", "何", "も", "分かっていない", "から",
                "そんな", "こと", "を", "言う", "ん", "です", "よ", "。", "弾ける", "もの", "なら", "弾いて", "みなさい", "。"
            ]
        );

        yield return new TestCaseData("", Array.Empty<string>());
    }
}