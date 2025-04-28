namespace music_api.Entities;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int PerformerId { get; set; }
    public Performer Performer { get; set; } = null!;
    public int? GenreId { get; set; }
    public Genre? Genre { get; set; }
    /// <summary>
    /// Час в секундах
    /// </summary>
    public int Duration { get; set; }
    public string? Album { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<PlaylistSong> PlaylistSongs { get; set; }
}