using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

/// <summary>
/// Handles individual word manipulation in isolation. Basically it's individual words, ignoring the
/// broader reading content context, like a word that a user adds it to their list of words known.
/// </summary>
public interface IWordService
{
    Task<Word?> GetOrCreateWordAsync(string text);
    Task<IEnumerable<Word>> GetWordsByTextAsync(IEnumerable<string> texts);
    Task<Word> CreateWordAsync(string text);
}