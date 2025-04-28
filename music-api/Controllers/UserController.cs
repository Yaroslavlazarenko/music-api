using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs.User;
using music_api.Features.Users.Commands;
using music_api.Features.Users.Queries;
using RegisterRequest = music_api.DTOs.User.RegisterRequest;

namespace music_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _mediator.Send(new RegisterUser.Command(request));
        
        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }
        
        return Ok(new { user = result.User, token = result.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] MyLoginRequest request)
    {
        var result = await _mediator.Send(new LoginUser.Command(request));
        
        if (!result.Success)
        {
            return Unauthorized();
        }
        
        return Ok(new { user = result.User, token = result.Token });
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] GetAllUsersRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllUsers.Query(request), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserById.Query(id), cancellationToken);
        
        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateUser.Command(id, dto), cancellationToken);
        
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUser.Command(id), cancellationToken);
        
        return NoContent();
    }
}