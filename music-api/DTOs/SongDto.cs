using System;

namespace music_api.DTOs;

public class SongDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PerformerId { get; set; }
    public string PerformerName { get; set; }
    public int? GenreId { get; set; }
    public string? GenreName { get; set; }
    /// <summary>
    /// Час у секундах
    /// </summary>
    public int Duration { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; }
}
