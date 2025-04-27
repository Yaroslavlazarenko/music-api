namespace music_api.Exceptions;

public class PerformerValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public PerformerValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
