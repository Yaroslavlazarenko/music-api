using AutoMapper;
using music_api.DTOs.Performer;
using music_api.Entities;

namespace music_api.Mappings;

public class PerformerProfile : Profile
{
    public PerformerProfile()
    {
        CreateMap<Performer, PerformerDto>();
        
        CreateMap<PerformerDto, Performer>();
        
        CreateMap<CreatePerformerDto, Performer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        
        CreateMap<UpdatePerformerDto, Performer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
