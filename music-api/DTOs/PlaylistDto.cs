using System;
using System.Collections.Generic;

namespace music_api.DTOs;

public class PlaylistDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublic { get; set; }
    public List<SongDto> Songs { get; set; } = new();
    /// <summary>
    /// Час у секундах
    /// </summary>
    public int TotalDuration { get; set; }
}
