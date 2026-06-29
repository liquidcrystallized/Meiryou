using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ja;
using Lucene.Net.Analysis.TokenAttributes;
using Meiryou.Core.Models;

namespace Meiryou.Core.Services.TextParsing;

public class JapaneseTextParsingService : ITextParsingService
{
    public LanguageType Language => LanguageType.Japanese;

    public JapaneseTextParsingService() { }

    //TODO: The default tokenizer doesn't output text in the required format, will need
    // a custom method to inflect and de-inflect verbs.
    public IEnumerable<string> SegmentTextIntoWords(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return [];
        
        Tokenizer tokenizer = new JapaneseTokenizer(new StringReader(text), null, false, JapaneseTokenizerMode.NORMAL);
        List<string> words = [];

        try
        {
            tokenizer.Reset();

            while (tokenizer.IncrementToken())
            {
                var charTermAttribute = tokenizer.GetAttribute<ICharTermAttribute>();
                
                words.Add(charTermAttribute.ToString());
            }

            tokenizer.End();
        }
        finally
        {
            tokenizer.Dispose();
        }

        return words;
    }
}