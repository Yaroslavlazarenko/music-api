using AutoMapper;
using music_api.DTOs;
using music_api.Entities;

namespace music_api.Mappings;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>();
        
        CreateMap<GenreDto, Genre>();
        
        CreateMap<CreateGenreDto, Genre>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        
        CreateMap<UpdateGenreDto, Genre>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}
