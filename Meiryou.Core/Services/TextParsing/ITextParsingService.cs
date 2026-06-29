using Meiryou.Core.Models;

namespace Meiryou.Core.Services.TextParsing;

public interface ITextParsingService
{
    /// <summary>
    /// Break a large string into independent "word" substrings.
    /// </summary>
    /// <param name="text">A large string to break up.</param>
    /// <returns>A collection of "word" strings.</returns>
    IEnumerable<string> SegmentTextIntoWords(string text);
    
    LanguageType Language { get; }
}