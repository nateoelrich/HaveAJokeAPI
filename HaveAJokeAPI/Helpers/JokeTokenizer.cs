using System.Text.RegularExpressions;
using HaveAJokeAPI.Models;

namespace HaveAJokeAPI.Helpers;

public static class JokeTokenizer
{
    private static readonly Regex WordPattern = new(@"\w+", RegexOptions.Compiled);
    
    public static Dictionary<JokeLengthCategory, List<string>> TransformJokes(ICollection<Joke> jokes, ICollection<string> tokens)
    {
        return GroupByLength(TokenizeJokes(jokes, tokens));
    }

    private static List<string> TokenizeJokes(ICollection<Joke> jokes, ICollection<string> searchTokens)
    {
        
        // TODO: ask "biz" if they want to pluralize search terms. I noticed the service returns pluralized matches: example search for 'cat'
        // TODO: I would also clarify with the "biz" if matches like "catholic" should be tokenized as well. Currently, they are not
        var tokenizedJokes = new List<string>();
        
        // Normalize search terms for case-insensitive comparison
        var searchSet = new HashSet<string>(searchTokens.Select(term => term.ToLowerInvariant()));
        
        // iterate over each joke and find search term matches with the regex.
        foreach (var joke in jokes)
        {
            // search the line with regex and find matching search terms
            var updatedLine = WordPattern.Replace(joke.Text, match =>
            {
                var word = match.Value;
                // when the word is in the search terms, transform it to uppercase
                return searchSet.Contains(word.ToLowerInvariant()) ? $"{word.ToUpper()}" : word;
            });
            tokenizedJokes.Add(updatedLine);
        }
        return tokenizedJokes;
    }
    
    private static Dictionary<JokeLengthCategory, List<string>> GroupByLength(List<string> strings)
    {
        return strings.GroupBy(GetLengthCategory).ToDictionary(g => g.Key, g => g.ToList());
    }

    private static JokeLengthCategory GetLengthCategory(string input)
    {
        var wordCount = WordPattern.Matches(input).Count;

        return wordCount switch
        {
            < 10 => JokeLengthCategory.Short,
            < 20 => JokeLengthCategory.Medium,
            _ => JokeLengthCategory.Long
        };
    }
}