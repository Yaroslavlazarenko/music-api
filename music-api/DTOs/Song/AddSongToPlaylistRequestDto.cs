namespace music_api.DTOs.Song
{
    public class AddSongToPlaylistRequestDto
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }
}
