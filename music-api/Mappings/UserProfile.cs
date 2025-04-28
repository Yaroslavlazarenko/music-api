using AutoMapper;
using music_api.DTOs.User;
using music_api.Entities;

namespace music_api.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        
        CreateMap<UserDto, User>();
    }
}
