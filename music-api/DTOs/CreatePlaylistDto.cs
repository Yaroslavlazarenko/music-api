namespace music_api.DTOs;

public class CreatePlaylistDto
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
}
