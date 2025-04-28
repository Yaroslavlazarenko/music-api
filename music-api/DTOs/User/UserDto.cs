namespace music_api.DTOs.User;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
