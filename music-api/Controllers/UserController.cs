using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs;
using music_api.Services.Users.Commands;
using music_api.Services.Users.Queries;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllUsers.Query(page, pageSize), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserById.Query(id), cancellationToken);

        if (user is null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateUser.Command(id, dto), cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteUser.Command(id), cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}