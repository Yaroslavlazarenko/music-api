namespace music_api.DTOs.Performer;

public class PerformerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
}
