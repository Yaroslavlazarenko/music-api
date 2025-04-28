namespace music_api.DTOs.Song;

public class CreateSongDto
{
    public string Title { get; set; } = null!;
    public int PerformerId { get; set; }
    public int? GenreId { get; set; }
    /// <summary>
    /// Час у секундах
    /// </summary>
    public int Duration { get; set; }
    public int? AlbumId { get; set; }
    public int? Year { get; set; }
}
