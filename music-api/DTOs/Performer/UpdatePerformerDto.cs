namespace music_api.DTOs.Performer;

public class UpdatePerformerDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Country { get; set; }
}
