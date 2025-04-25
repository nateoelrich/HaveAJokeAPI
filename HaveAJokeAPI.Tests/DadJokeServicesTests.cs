using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;
using HaveAJokeAPI.Services;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace HaveAJokeAPI.Tests;

public class DadJokeServicesTests
{
    private Mock<IDadJokeRepository> _jokeRepository;
    
    [SetUp]
    public void SetUp()
    {
        _jokeRepository = new Mock<IDadJokeRepository>();
    }

    [Test]
    public async Task GivenNonNullResponse_GetRandom_ShouldReturnJoke()
    {
        // Arrange
        var maJoke = new Joke()
        {
            Text = "I am a joke",
        };
        
        _jokeRepository.Setup(x => x.GetRandomJokeAsync()).ReturnsAsync(maJoke);
        
        var jokeService = new JokeService(_jokeRepository.Object);
        
        // Act
        var result = await jokeService.GetRandomJokeAsync();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Joke>();
        result.Text.ShouldBe(maJoke.Text);
    }

    [Test]
    public async Task GivenNullResponse_GetRandom_ShouldThrow()
    {
        // Arrange
        Joke? maJoke = null;

        _jokeRepository.Setup(x => x.GetRandomJokeAsync()).ReturnsAsync(maJoke);

        var jokeService = new JokeService(_jokeRepository.Object);

        // Act
        // Assert
        Assert.ThrowsAsync<JokeNotFoundException>(async () => await jokeService.GetRandomJokeAsync());
    }
}