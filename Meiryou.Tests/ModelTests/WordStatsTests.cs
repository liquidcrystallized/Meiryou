using Meiryou.Models;

namespace Meiryou.Tests.ModelTests;

[TestFixture]
public class WordStatsTests
{
    [Test]
    public void Constructor_InitialisesWithDefaultValues()
    {
        var stats = new WordStats();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(stats.Definition, Is.Empty);
            Assert.That(stats.PartOfSpeech, Is.Empty);
            Assert.That(stats.FrequencyRank, Is.EqualTo(-1));
            Assert.That(stats.WordFamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.Unknown));
        }
    }

    [Test]
    public void DefinitionProperty_SetValue_DefinitionIsUpdated()
    {
        var stats = new WordStats { Definition = "A test definition" };
        
        Assert.That(stats.Definition, Is.EqualTo("A test definition"));
    }

    [Test]
    public void PartOfSpeechProperty_SetValue_PartOfSpeechIsUpdated()
    {
        var stats = new WordStats { PartOfSpeech = "Noun" };
        
        Assert.That(stats.PartOfSpeech, Is.EqualTo("Noun"));
    }

    [Test]
    public void FrequencyRankProperty_SetValue_FrequencyRankIsUpdated()
    {
        var stats = new WordStats { FrequencyRank = 67 };
        
        Assert.That(stats.FrequencyRank, Is.EqualTo(67));
    }

    [Test]
    public void WordFamiliarityLevelProperty_SetValue_WordFamiliarityLevelIsUpdated()
    {
        var stats = new WordStats { WordFamiliarityLevel = WordFamiliarityLevel.New };

        Assert.That(stats.WordFamiliarityLevel, Is.EqualTo(WordFamiliarityLevel.New));
    }
}