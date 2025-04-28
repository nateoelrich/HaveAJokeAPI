namespace HaveAJokeAPI.Exceptions;

public class JokeNotFoundException(string message) 
    : Exception(message)
{
    
}