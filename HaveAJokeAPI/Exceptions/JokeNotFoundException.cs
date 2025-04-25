namespace HaveAJokeAPI.Exceptions;

public class JokeNotFoundException(string message) 
    : Exception($"Womp Womp: {message}")
{
    
}