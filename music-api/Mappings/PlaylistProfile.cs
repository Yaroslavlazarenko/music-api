using AutoMapper;
using music_api.DTOs;
using music_api.Entities;

namespace music_api.Mappings;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<Playlist, PlaylistDto>()
            .ForMember(d => d.Songs, opt => opt.MapFrom(s => s.Songs))
            .ForMember(d => d.TotalDuration, opt => opt.MapFrom(s => s.Songs != null ? s.Songs.Sum(song => song.Duration) : 0));
        
        CreateMap<PlaylistDto, Playlist>()
            .ForMember(d => d.Songs, opt => opt.Ignore());
        
        CreateMap<CreatePlaylistDto, Playlist>()
            .ForMember(d => d.Songs, opt => opt.Ignore());
        
        CreateMap<UpdatePlaylistDto, Playlist>()
            .ForMember(d => d.Songs, opt => opt.Ignore());
    }
}
