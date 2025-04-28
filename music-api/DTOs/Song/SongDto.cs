namespace music_api.DTOs.Song;

public class SongDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PerformerId { get; set; }
    public string PerformerName { get; set; } = null!;
    public int? GenreId { get; set; }
    public string? GenreName { get; set; }
    /// <summary>
    /// Час у секундах
    /// </summary>
    public int Duration { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; }
}
