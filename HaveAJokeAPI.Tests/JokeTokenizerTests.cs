using HaveAJokeAPI.Helpers;
using HaveAJokeAPI.Models;
using NUnit.Framework;
using Shouldly;

namespace HaveAJokeAPI.Tests;

[TestFixture]
public class JokeTokenizerTests
{
    [Test]
    public void TransformJokes_EmptyJokes_ReturnsEmptyDictionary()
    {
        var jokes = new List<Joke>();
        var tokens = new List<string> { "cat", "dog" };

        var result = JokeTokenizer.TransformJokes(jokes, tokens);

        result.ShouldBeEmpty();
    }

    [Test]
    public void TransformJokes_NoMatchingTokens_JokesUnchanged_GroupedByLength()
    {
        var jokes = new List<Joke>
        {
            new Joke { Text = "This is a random joke." },
            new Joke { Text = "Another one without the token. And Another one without the token." }
        };
        var tokens = new List<string> { "unicorn", "wizard" };

        var result = JokeTokenizer.TransformJokes(jokes, tokens);

        result.ShouldContainKey(JokeLengthCategory.Short);
        result.ShouldContainKey(JokeLengthCategory.Medium);

        result[JokeLengthCategory.Short].Count.ShouldBe(1);
        result[JokeLengthCategory.Medium].Count.ShouldBe(1);

        result.Values.SelectMany(x => x).Any(j => j.Contains("unicorn")).ShouldBeFalse();
        result.Values.SelectMany(x => x).Any(j => j.Contains("wizard")).ShouldBeFalse();
    }

    [Test]
    public void TransformJokes_MatchingTokens_UppercasesMatchedWords()
    {
        var jokes = new List<Joke>
        {
            new Joke { Text = "I have a cat and a dog." }
        };
        var tokens = new List<string> { "cat", "dog" };

        var result = JokeTokenizer.TransformJokes(jokes, tokens);

        var updatedJoke = result[JokeLengthCategory.Short].First();
        updatedJoke.ShouldContain("CAT");
        updatedJoke.ShouldContain("DOG");
    }

    [Test]
    public void TransformJokes_CaseInsensitiveTokenMatch_UppercasesRegardlessOfCase()
    {
        var jokes = new List<Joke>
        {
            new Joke { Text = "My Dog is cute. I love my CAT." }
        };
        var tokens = new List<string> { "dog", "cat" };

        var result = JokeTokenizer.TransformJokes(jokes, tokens);

        var updatedJoke = result[JokeLengthCategory.Short].First();
        updatedJoke.ShouldContain("DOG");
        updatedJoke.ShouldContain("CAT");
    }

    [Test]
    public void TransformJokes_WordBoundaryMatching_DoesNotUppercasePartials()
    {
        var jokes = new List<Joke>
        {
            new Joke { Text = "I am catholic." }
        };
        var tokens = new List<string> { "cat" };

        var result = JokeTokenizer.TransformJokes(jokes, tokens);

        var updatedJoke = result[JokeLengthCategory.Short].First();
        updatedJoke.ShouldNotBe("I am CATholic");
    }
}