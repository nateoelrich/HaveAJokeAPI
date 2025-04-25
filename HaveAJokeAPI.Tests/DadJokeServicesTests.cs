using HaveAJokeAPI.Exceptions;
using HaveAJokeAPI.Models;
using HaveAJokeAPI.Repositories;
using HaveAJokeAPI.Responses;
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
        result.ShouldBeOfType<JokeResponse>();
        result.Line.ShouldBe(maJoke.Text);
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

    [Test]
    public async Task GivenRating_GetRandom_ShouldCrush()
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

        // i would have lots of apprehension about doing this sort of check in a test irl, but I thought it would be cute
        // if the response randomly rated the joke and whether it would crush
        if (result.Rating > 0.8m)
        {
            result.DidCrush.ShouldBeTrue();
        }
        
        result.DidCrush.ShouldBeFalse();
    }
}