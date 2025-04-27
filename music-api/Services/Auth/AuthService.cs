using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using music_api.Entities;
using music_api.Settings;
using music_api.DTOs;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using music_api.Controllers;
using RegisterRequest = music_api.DTOs.RegisterRequest;

namespace music_api.Services.Auth;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
    Task<LoginResult> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IOptions<JwtSettings> jwtOptions, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtOptions.Value;
        _mapper = mapper;
    }

    public async Task<RegisterResult> RegisterAsync(RegisterRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            CreatedAt = DateTime.UtcNow,
            Playlists = new List<Playlist>()
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new RegisterResult
            {
                Success = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }
        
        var userDto = _mapper.Map<UserDto>(user);
        var token = GenerateJwtToken(user);
        
        return new RegisterResult
        {
            Success = true,
            User = userDto,
            Token = token
        };
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            return new LoginResult { Success = false };
        }
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        
        if (!result.Succeeded)
        {
            return new LoginResult { Success = false };
        }
        
        var userDto = _mapper.Map<UserDto>(user);
        var token = GenerateJwtToken(user);
        
        return new LoginResult
        {
            Success = true,
            User = userDto,
            Token = token
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
