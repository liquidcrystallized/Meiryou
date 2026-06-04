using Meiryou.Core.Models;

namespace Meiryou.Core.Services;

public interface IReadingContentService
{
    Task<IEnumerable<ReadingContent>> GetAllContentsAsync();
    Task<ReadingContent?> GetContentByIdAsync(int id);
    Task<ReadingContent> AddContentAsync(string title, string content);
    Task DeleteContentAsync(int id);
    Task<IEnumerable<Word>> GetWordsInContentAsync(int contentId);
    Task<IEnumerable<SentenceContext>> GetSentenceContextsAsync(int wordId);
    Task<Word?> GetOrCreateWordAsync(string text);
}