using Meiryou.Core.Services.TextParsing;

namespace Meiryou.Tests.UnitTests.Core.Services.TextParsing;

[TestFixture]
public class JapaneseTextParsingServiceTests : TextParsingServiceSharedTests
{
    protected override ITextParsingService CreateService()
    {
        return new JapaneseTextParsingService();
    }

    [Test]
    public void SegmentTextIntoWords_ShouldReturnCorrectListOfWords()
    {
        string sentence = "この曲がやさしいって言うんですか？何も分かっていないからそんなことを言うんですよ。弾けるものなら弾いてみなさい。";
        List<string> expectedWords =
        [
            "この", "曲", "が", "やさしい", "って", "言う", "ん", "です", "か", "？", "何", "も", "分かっていない", "から", 
            "そんな", "こと", "を", "言う", "ん", "です", "よ", "。", "弾ける", "もの", "なら", "弾いて", "みなさい", "。"
        ];
        
        List<string> actualWords = Service.SegmentTextIntoWords(sentence).ToList();
        
        Assert.That(actualWords, Is.EqualTo(expectedWords));
    }
}