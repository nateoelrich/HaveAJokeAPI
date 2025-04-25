using HaveAJokeAPI.Models;

namespace HaveAJokeAPI.Repositories;

public interface IDadJokeRepository
{
    Task<Joke?> GetRandomJoke();
    Task<ICollection<Joke>>  SearchJokes(string jokeToken);
}