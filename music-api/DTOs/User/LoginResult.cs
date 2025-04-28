namespace music_api.DTOs.User;

public class LoginResult
{
    public bool Success { get; set; }
    public UserDto User { get; set; } = null!;
    public string Token { get; set; } = null!;
}
