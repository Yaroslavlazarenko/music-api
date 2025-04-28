using System;
using System.Collections.Generic;

namespace music_api.Exceptions;

public class PlaylistValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }
    public PlaylistValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }
}
