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
        // Normalize search terms for case-insensitive comparison
        var searchSet = new HashSet<string>(
            searchTokens.Select(term => term.ToLowerInvariant())
        );

        
        // TODO: maybe convert this to plain old for loop for better understanding
        return jokes.Select(j =>
        {
            // Replace words if they match any search term
            return WordPattern.Replace(j.Text, match =>
            {
                var word = match.Value;
                return searchSet.Contains(word.ToLowerInvariant()) ? $"{word.ToUpper()}" : word;
            });
        }).ToList();
    }
    
    private static Dictionary<JokeLengthCategory, List<string>> GroupByLength(List<string> strings)
    {
        return strings.GroupBy(GetLengthCategory)
            .ToDictionary(g => g.Key, g => g.ToList());
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