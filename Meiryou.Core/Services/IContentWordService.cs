using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

/// <summary>
/// Handles words inside the content. So it's the content in the reader, not the
/// individual stuff handled in IWordService.
/// </summary>
public interface IContentWordService
{
    Task<IEnumerable<Word>> GetWordsInContentAsync(int contentId);
}