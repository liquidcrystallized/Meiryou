using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

/// <summary>
/// Manages content lifecycle of a piece of reading content. So basic deletion
/// and import of whole texts into the application, and stuff managing it.
/// </summary>
public interface IReadingContentService
{
    Task<IEnumerable<ReadingContent>> GetAllContentsAsync();
    Task<ReadingContent?> GetContentByIdAsync(int id);
    Task<ReadingContent> ImportContentAsync(LanguageType language, string title, string content);
    Task DeleteContentAsync(int id);
}