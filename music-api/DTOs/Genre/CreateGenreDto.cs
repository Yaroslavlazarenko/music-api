namespace music_api.DTOs.Genre;

public class CreateGenreDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
