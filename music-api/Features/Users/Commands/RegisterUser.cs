using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using music_api.DTOs.User;
using music_api.Entities;
using music_api.Settings;

namespace music_api.Features.Users.Commands;

public static class RegisterUser
{
    public record Command(RegisterRequest Request) : IRequest<RegisterResult>;

    public class Handler : IRequestHandler<Command, RegisterResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public Handler(UserManager<User> userManager, IMapper mapper, IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<RegisterResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Request.Email,
                UserName = request.Request.UserName,
                CreatedAt = DateTime.UtcNow,
                Playlists = new List<Playlist>()
            };

            var result = await _userManager.CreateAsync(user, request.Request.Password);

            if (!result.Succeeded)
            {
                return new RegisterResult
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            var userDto = _mapper.Map<UserDto>(user);
            var token = JwtTokenHelper.GenerateJwtToken(user, _jwtSettings);

            return new RegisterResult
            {
                Success = true,
                User = userDto,
                Token = token
            };
        }
    }
}
