using Meiryou.Core.Models;

namespace Meiryou.Core.Services.TextParsing;

public class TextParsingServiceFactory : ITextParsingServiceFactory
{
    private readonly IEnumerable<ITextParsingService> _services;

    public TextParsingServiceFactory(IEnumerable<ITextParsingService> services)
    {
        _services = services;
    }
    
    public ITextParsingService GetService(LanguageType language)
    {
        return _services.FirstOrDefault(s => s.Language == language)
               ?? throw new InvalidOperationException($"No parsing service found for {language}");
    }
}