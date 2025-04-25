using System.Runtime.InteropServices;
using System.Text.Json;
using Flurl.Http.Testing;
using HaveAJokeAPI.Configuration;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;
using NUnit.Framework;
using Shouldly;

namespace HaveAJokeAPI.Tests;

public class DadJokeRepositoryTests
{
    private DadJokeApiConfiguration _configuration;
    private DadJokeRepository _repository;
    private Joke _defaultJoke;

    [SetUp]
    public void Setup()
    {
        _configuration = new DadJokeApiConfiguration()
        {
            BaseUrl = "http://hihungry.com/imdad",
            AcceptHeader = "application/json",
        };
        
        _repository = new DadJokeRepository(_configuration);
        _defaultJoke = new Joke()
        {
            Id = "'mclovin",
            Text = "Why are pigs bad drivers? ...`cause they hog the road!",
            Status = "200"
        };
    }
    
    [Test]
    public async Task GivenSuccessStatusCode_GetRandom_ShouldReturnRandomJoke()
    {
        using var client = new HttpTest();

        client.RespondWith(JsonSerializer.Serialize(_defaultJoke)); // <- default status is 200

        await _repository.GetRandomJoke();

        client.ShouldHaveCalled(_configuration.BaseUrl)
            .WithHeader("Accept", _configuration.AcceptHeader)
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }
    
    [Test]
    public async Task GivenNonSuccessStatusCode_GetRandom_ShouldReturnNull()
    {
        using var client = new HttpTest();

        var badJoke = new Joke()
        {
            Id = string.Empty,
            Text = string.Empty,
            Status = "500"
        };

        client.RespondWith(JsonSerializer.Serialize(badJoke));

        var result = await _repository.GetRandomJoke();

        client.ShouldHaveCalled(_configuration.BaseUrl)
            .WithHeader("Accept", _configuration.AcceptHeader)
            .WithVerb(HttpMethod.Get)
            .Times(1);

        result.ShouldBeNull();
    }
    
    [Test]
    public async Task GivenApiDependencyFailure_GetRandom_ShouldReturnNull()
    {
        using var client = new HttpTest();

        client.SimulateTimeout();

        var result = await _repository.GetRandomJoke();

        client.ShouldHaveCalled(_configuration.BaseUrl)
            .WithHeader("Accept", _configuration.AcceptHeader)
            .WithVerb(HttpMethod.Get)
            .Times(1);
        
        result.ShouldBeNull();
    }

    [Test]
    public async Task GivenSuccessStatusCode_Search_ShouldReturnListOfJokes()
    {
        throw new NotImplementedException();
    }
    
    [Test]
    public async Task GivenNonSuccessStatusCode_Search_ShouldReturnRandomJoke()
    {
        throw new NotImplementedException();
    }
}