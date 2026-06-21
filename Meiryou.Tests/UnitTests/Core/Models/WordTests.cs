using Meiryou.Core.Models;

namespace Meiryou.Tests.UnitTests.Core.Models;

[TestFixture]
public class WordTests
{
    [Test]
    public void Constructor_InitialisesWithDefaultValues()
    {
        var word = new Word();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(word.Id, Is.EqualTo(0)); // Default int
            Assert.That(word.Text, Is.Empty);
            Assert.That(word.Definition, Is.Empty);
            Assert.That(word.PartOfSpeech, Is.Empty);
            Assert.That(word.FrequencyRank, Is.EqualTo(-1));
            Assert.That(word.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Unknown));
        }
    }

    [Test]
    public void Property_Setters_UpdateValues()
    {
        var word = new Word
        {
            Text = "Test",
            Definition = "A sample",
            FrequencyRank = 100,
            FamiliarityLevel = WordFamiliarityLevel.Known
        };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(word.Text, Is.EqualTo("Test"));
            Assert.That(word.Definition, Is.EqualTo("A sample"));
            Assert.That(word.FrequencyRank, Is.EqualTo(100));
            Assert.That(word.FamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Known));
        }
    }
}