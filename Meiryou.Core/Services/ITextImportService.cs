namespace Meiryou.Core.Services;

public interface ITextImportService
{
    Task ImportTextAsync(string title, string content);
}