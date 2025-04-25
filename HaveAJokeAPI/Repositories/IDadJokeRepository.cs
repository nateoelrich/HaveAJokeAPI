namespace HaveAJokeAPI.Repositories;

public interface IDadJokeRepository
{
    Task GetRandomJoke();
    Task SearchJokes(string jokeToken);
}