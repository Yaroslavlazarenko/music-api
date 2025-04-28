using AutoMapper;
using music_api.DTOs.Playlist;
using music_api.Entities;

namespace music_api.Mappings;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<Playlist, PlaylistDto>()
            .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.PlaylistSongs.Select(ps => ps.Song)))
            .ForMember(d => d.TotalDuration, opt => opt.MapFrom(s => s.PlaylistSongs.Sum(ps => ps.Song.Duration)));
        
        CreateMap<PlaylistDto, Playlist>();
        
        CreateMap<CreatePlaylistDto, Playlist>()
            .ForMember(d => d.PlaylistSongs, opt => opt.Ignore());
        
        CreateMap<UpdatePlaylistDto, Playlist>()
            .ForMember(d => d.PlaylistSongs, opt => opt.Ignore());
    }
}
