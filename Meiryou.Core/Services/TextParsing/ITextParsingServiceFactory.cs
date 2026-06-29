using Meiryou.Core.Models;

namespace Meiryou.Core.Services.TextParsing;

public interface ITextParsingServiceFactory
{
    ITextParsingService GetService(LanguageType language);
}