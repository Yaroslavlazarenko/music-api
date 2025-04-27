namespace music_api.Exceptions;

public class GenreValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public GenreValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
