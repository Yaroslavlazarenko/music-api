namespace music_api.DTOs.User;

public class UpdateUserDto
{
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
}
