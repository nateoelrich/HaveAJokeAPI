using HaveAJokeAPI.Configuration;

namespace HaveAJokeAPI.Repositories;

public class DadJokeRepository : IDadJokeRepository
{

    public DadJokeRepository(DadJokeApiConfiguration configuration)
    {
        
    }
    
    public Task GetRandomJoke()
    {
        throw new NotImplementedException();
    }

    public Task SearchJokes(string jokeToken)
    {
        throw new NotImplementedException();
    }
}