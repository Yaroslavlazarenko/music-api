namespace music_api.DTOs.User;

public class RegisterResult
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public UserDto? User { get; set; }
    public string? Token { get; set; }
}