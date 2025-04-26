using System.Text.RegularExpressions;
using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;

namespace HaveAJokeAPI.Services;

public interface IJokeService
{
    Task<Joke> GetRandomJokeAsync();
    
    Task<List<Joke>> SearchJokesAsync(int page, int limit, string token);
}

/// <summary>
/// The best service around who's a good service, you are!
/// </summary>
public class JokeService : IJokeService
{
    private readonly IDadJokeRepository _repository;
    private static readonly Regex WordPattern = new(@"\w+", RegexOptions.Compiled);

    /// <summary>
    /// Inits a JokeService
    /// </summary>
    /// <param name="repository"><see cref="IDadJokeRepository"/></param>
    public JokeService(IDadJokeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a random dad joke with a random rating
    /// </summary>
    /// <returns><see cref="JokeResponse"/></returns>
    /// <exception cref="JokeNotFoundException">When no joke is found</exception>
    public async Task<Joke> GetRandomJokeAsync()
    {
        var joke = await _repository.GetRandomJokeAsync();

        if (joke == null)
        {
            throw new JokeNotFoundException("Ain't nobody got time for this!");
        }

        return joke;
    }

    public async Task<List<Joke>> SearchJokesAsync(int page, int limit, string token)
    {
        var jokes = await _repository.SearchJokesAsync(page, limit, token);

        if (jokes == null)
        {
            throw new JokeNotFoundException("try again later!");
        }

        return TransformJokes(jokes, token.Split(" "));
    }

    private List<Joke> TransformJokes(ICollection<Joke> jokes, ICollection<string> tokens)
    {
        var tokenizedJokes = TokenizeJokes(jokes, tokens);
        return GroupByLength(tokenizedJokes);
    }

    private object TokenizeJokes(ICollection<Joke> jokes, ICollection<string> searchTokens)
    { 
        if (jokes == null || searchTokens == null)
            return jokes;

        // Normalize search terms for case-insensitive comparison
        var searchSet = new HashSet<string>(
            searchTokens.Select(term => term.ToLowerInvariant())
        );

        var wordPattern = new Regex(@"\w+", RegexOptions.Compiled);

        return jokes.Select(j =>
        {
            // Replace words if they match any search term
            return wordPattern.Replace(j.Text, match =>
            {
                var word = match.Value;
                return searchSet.Contains(word.ToLowerInvariant()) ? $"{word.ToUpper()}" : word;
            });
        }).ToList();
    }
    
    private static Dictionary<LengthCategory, List<string>> GroupByLength(List<string> strings)
    {
        return strings.GroupBy(GetLengthCategory)
            .ToDictionary(g => g.Key, g => g.ToList());
    }

    private static LengthCategory GetLengthCategory(string input)
    {
        var wordCount = WordPattern.Matches(input).Count;

        return wordCount switch
        {
            < 10 => LengthCategory.Short,
            < 20 => LengthCategory.Medium,
            _ => LengthCategory.Long
        };
    }
    
    public enum LengthCategory
    {
        Short,
        Medium,
        Long
    }

}