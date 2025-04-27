namespace music_api.Exceptions;

public record ValidationError(string Field, string Description);
