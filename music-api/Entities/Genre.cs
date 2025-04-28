namespace music_api.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}