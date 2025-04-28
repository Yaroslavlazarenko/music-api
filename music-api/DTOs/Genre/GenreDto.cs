namespace music_api.DTOs.Genre;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
