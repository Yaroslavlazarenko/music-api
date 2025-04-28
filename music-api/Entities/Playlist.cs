using System;
using System.Collections.Generic;

namespace music_api.Entities;

public class Playlist
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublic { get; set; }
    public ICollection<PlaylistSong> PlaylistSongs { get; set; }
}