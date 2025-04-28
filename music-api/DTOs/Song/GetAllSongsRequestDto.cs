namespace music_api.DTOs.Song
{
    public class GetAllSongsRequestDto
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int? PerformerId { get; set; }
        public int? GenreId { get; set; }
    }
}
