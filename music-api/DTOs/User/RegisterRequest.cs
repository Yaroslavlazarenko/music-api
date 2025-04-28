namespace music_api.DTOs.User;

public class RegisterRequest
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Email { get; set; }
}
