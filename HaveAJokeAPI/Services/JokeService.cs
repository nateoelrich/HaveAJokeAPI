using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;
using HaveAJokeAPI.Responses;

namespace HaveAJokeAPI.Services;

public interface IJokeService
{
    Task<JokeResponse> GetRandomJokeAsync();
    
    Task<List<JokeResponse>> SearchJokesAsync(int page, int limit, string token);
}

/// <summary>
/// The best service around who's a good service, you are!
/// </summary>
public class JokeService : IJokeService
{
    private readonly IDadJokeRepository _repository;
    private readonly Random _random;

    /// <summary>
    /// Inits a JokeService
    /// </summary>
    /// <param name="repository"><see cref="IDadJokeRepository"/></param>
    public JokeService(IDadJokeRepository repository)
    {
        _repository = repository;
        _random = new Random();
    }

    /// <summary>
    /// Gets a random dad joke with a random rating
    /// </summary>
    /// <returns><see cref="JokeResponse"/></returns>
    /// <exception cref="JokeNotFoundException">When no joke is found</exception>
    public async Task<JokeResponse> GetRandomJokeAsync()
    {
        var joke = await _repository.GetRandomJokeAsync();

        if (joke == null)
        {
            throw new JokeNotFoundException("Ain't nobody got time for this!");
        }
        
        return JokeResponse(joke);
    }

    public async Task<List<JokeResponse>> SearchJokesAsync(int page, int limit, string token)
    {
        var jokes = await _repository.SearchJokesAsync(page, limit, token);

        if (jokes == null)
        {
            throw new JokeNotFoundException("try again later!");
        }
        
        return CategorizeJokes(jokes);
    }

    private List<JokeResponse> CategorizeJokes(ICollection<Joke> jokes)
    {
        // TODO: add all the sorting and token highlight here
        var jokies = new List<JokeResponse>();
        foreach (var joke in jokes)
        {
            jokies.Add(JokeResponse(joke));
        }
        return jokies;
    }

    /// <summary>
    /// Simulate some biz logic
    /// </summary>
    /// <param name="joke">A joke with a random rating a bool if that score is over 80%.</param>
    /// <returns><see cref="JokeResponse"/></returns>
    private JokeResponse JokeResponse(Joke joke)
    {
        var score = _random.Next(0, 100);
        var rating = score / 100m;
        
        return new JokeResponse()
        {
            Line = joke.Text,
            Rating = score/100m,
            DidCrush = rating > 0.8m
        };
    }
}