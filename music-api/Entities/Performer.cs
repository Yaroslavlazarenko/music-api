namespace music_api.Entities;

public class Performer
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}