using Flurl.Http.Testing;
using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Repositories;
using NUnit.Framework;

namespace HaveAJokeAPI.Tests;

public class DadJokeRepositoryTests
{
    private DadJokeApiConfiguration _configuration;
    private DadJokeRepository _repository;

    [SetUp]
    public void Setup()
    {
        _configuration = new DadJokeApiConfiguration()
        {
            BaseUrl = "http://hihungryimdad.com",
        };
        
        _repository = new DadJokeRepository(_configuration);
    }
    
    [Test]
    public async Task GivenSuccessStatusCode_GetRandom_ShouldReturnRandomJoke()
    {
        using var client = new HttpTest();

        client.RespondWith("{}", 200);

        var result = _repository.GetRandomJoke();

        client.ShouldHaveCalled(_configuration.BaseUrl)
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }
    
    [Test]
    public async Task GivenNonSuccessStatusCode_GetRandom_ShouldReturnRandomJoke()
    {
        
    }

    [Test]
    public async Task GivenSuccessStatusCode_Search_ShouldReturnListOfJokes()
    {
        
    }
    
    [Test]
    public async Task GivenNonSuccessStatusCode_Search_ShouldReturnRandomJoke()
    {
        
    }
}