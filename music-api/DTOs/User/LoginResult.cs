namespace music_api.DTOs.User;

public class LoginResult
{
    public bool Success { get; set; }
    public UserDto User { get; set; }
    public string Token { get; set; }
}
