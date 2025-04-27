namespace music_api.Exceptions;

public class SongValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public SongValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
