using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using music_api.DTOs;
using music_api.Services.Auth;

namespace music_api.Controllers;


public record LoginRequest(string Email, string Password);

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(new { user = result.User, token = result.Token });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        
        if (!result.Success)
        {
            return Unauthorized();
        }
        
        return Ok(new { user = result.User, token = result.Token });
    }
}

