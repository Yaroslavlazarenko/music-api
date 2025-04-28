namespace music_api.DTOs.Playlist;

public class CreatePlaylistDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
}
