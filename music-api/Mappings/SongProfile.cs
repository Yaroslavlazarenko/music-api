using AutoMapper;
using music_api.DTOs;
using music_api.Entities;

namespace music_api.Mappings;

public class SongProfile : Profile
{
    public SongProfile()
    {
        CreateMap<Song, SongDto>()
            .ForMember(d => d.PerformerName, opt => opt.MapFrom(s => s.Performer.Name))
            .ForMember(d => d.GenreName, opt => opt.MapFrom(s => s.Genre != null ? s.Genre.Name : null))
            .ForMember(d => d.Duration, opt => opt.MapFrom(s => s.Duration));

        CreateMap<SongDto, Song>()
            .ForMember(d => d.Duration, opt => opt.MapFrom(s => s.Duration));

        CreateMap<CreateSongDto, Song>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(s => s.Duration));

        CreateMap<UpdateSongDto, Song>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(s => s.Duration));
    }
}
