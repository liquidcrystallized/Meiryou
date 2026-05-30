using Meiryou.Models;

namespace Meiryou.Tests.App.ModelTests;

[TestFixture]
public class WordDataTests
{
    [Test]
    public void Constructor_InitialState_TextIsEmpty()
    {
        var wordData = new WordData();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(wordData.Text, Is.Empty);
            Assert.That(wordData.Length, Is.Zero);
            Assert.That(wordData.IsSpace, Is.False);
        }
    }

    [Test]
    public void TextProperty_SetValue_TextIsUpdated()
    {
        var wordData = new WordData { Text = "テスト" };

        Assert.That(wordData.Text, Is.EqualTo("テスト"));
    }

    [Test]
    public void LengthProperty_WithJapaneseText_ReturnsCorrectLength()
    {
        var wordData = new WordData { Text = "日本語" };

        Assert.That(wordData.Length, Is.EqualTo(3));
    }

    [Test]
    public void LengthProperty_WithMixedText_ReturnsCorrectLength()
    {
        var wordData = new WordData { Text = "Hello 世界" };
        
        Assert.That(wordData.Length, Is.EqualTo(8));
    }

    [Test]
    public void IsSpace_Property_WithSingleSpace_ReturnsTrue()
    {
        var wordData = new WordData { Text = " " };
        
        Assert.That(wordData.IsSpace, Is.True);
    }

    [Test]
    public void IsSpace_Property_WithFullWidthSpace_ReturnsTrue()
    {
        var wordData = new WordData { Text = "　" };
        
        Assert.That(wordData.IsSpace, Is.True);
    }

    [Test]
    public void IsSpace_Property_WithEmptyString_ReturnsFalse()
    {
        var wordData = new WordData { Text = "" };
        
        Assert.That(wordData.IsSpace, Is.False);
    }

    [Test]
    public void IsSpace_Property_WithNormalText_ReturnsFalse()
    {
        var wordData = new WordData { Text = "test" };
        
        Assert.That(wordData.IsSpace, Is.False);
    }

    [Test]
    public void Equals_SameTextAndSpace_ReturnsTrue()
    {
        var wordData1 = new WordData { Text = "test" };
        var wordData2 = new WordData { Text = "test" };

        Assert.That(wordData1.Equals(wordData2), Is.True);
    }

    [Test]
    public void Equals_DifferentText_ReturnsFalse()
    {
        var wordData1 = new WordData { Text = "test1" };
        var wordData2 = new WordData { Text = "test2" };

        Assert.That(wordData1.Equals(wordData2), Is.False);
    }

    [Test]
    public void Equals_DifferentSpace_ReturnsFalse()
    {
        var wordData1 = new WordData { Text = " " };
        var wordData2 = new WordData { Text = "　" };
    
        Assert.That(wordData1.Equals(wordData2), Is.False);
    }

    [Test]
    public void Equals_WithNull_ReturnsFalse()
    {
        var wordData = new WordData { Text = "test" };
        
        Assert.That(wordData.Equals(null), Is.False);
    }

    [Test]
    public void GetHashCode_SameText_ReturnsSameHashCode()
    {
        var wordData1 = new WordData { Text = "test" };
        var wordData2 = new WordData { Text = "test" };
        
        Assert.That(wordData1.GetHashCode(), Is.EqualTo(wordData2.GetHashCode()));
    }

    [Test]
    public void Equals_WithObject_ReturnsCorrectResult()
    {
        var wordData1 = new WordData { Text = "test" };
        object obj1 = new WordData { Text = "test" };
        object obj2 = new WordData { Text = "different" };
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(wordData1.Equals(obj1), Is.True);
            Assert.That(wordData1.Equals(obj2), Is.False);
        }
    }
}