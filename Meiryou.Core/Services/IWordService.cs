using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

/// <summary>
/// Handles individual word manipulation in isolation. Basically it's individual words, ignoring the
/// broader reading content context, like a word that a user adds it to their list of words known.
/// </summary>
public interface IWordService
{
    /// <summary>
    /// Get the existing words that a user already knows.
    /// </summary>
    /// <param name="texts">A list of (preferably) unique strings.</param>
    /// <returns></returns>
    Task<IEnumerable<Word>> GetWordsByTextAsync(IEnumerable<string> texts);
    
    /// <summary>
    /// Updates a word if it already exists (Just the familiarity level).
    /// Add the new word/word if it doesn't.
    /// </summary>
    /// <param name="text">A string representation of a word.</param>
    /// <param name="familiarityLevel"> How familiar a user is with a word.</param>
    /// <returns></returns>
    Task<Word> SaveWordAsync(string text, WordFamiliarityLevel familiarityLevel = WordFamiliarityLevel.Unknown);
}