using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using music_api.DTOs.User;
using music_api.Entities;
using music_api.Settings;

namespace music_api.Features.Users.Commands;

public static class LoginUser
{
    public record Command(LoginRequest Request) : IRequest<LoginResult>;

    public class Handler : IRequestHandler<Command, LoginResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IMapper _mapper;

        public Handler(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JwtSettings> jwtOptions, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtOptions.Value;
            _mapper = mapper;
        }

        public async Task<LoginResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Request.Email);
            if (user is null)
                return new LoginResult { Success = false };

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Request.Password, false);
            if (!result.Succeeded)
                return new LoginResult { Success = false };

            var userDto = _mapper.Map<UserDto>(user);
            var token = JwtTokenHelper.GenerateJwtToken(user, _jwtSettings);

            return new LoginResult
            {
                Success = true,
                User = userDto,
                Token = token
            };
        }
    }
}


public static class JwtTokenHelper
{
    public static string GenerateJwtToken(User user, JwtSettings jwtSettings)
    {
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, user.UserName ?? ""),
            new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? "")
        };
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes),
            signingCredentials: creds
        );
        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
