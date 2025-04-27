using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs;
using music_api.Services.Performers.Commands;
using music_api.Services.Performers.Queries;

namespace music_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PerformerController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PerformerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PerformerDto>> Add([FromBody] CreatePerformerDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddPerformer.Command(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PerformerDto>>> GetAll(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllPerformers.Query(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PerformerDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var performer = await _mediator.Send(new GetPerformerById.Query(id), cancellationToken);
        
        if (performer is null)
        {
            return NotFound();
        }
        
        return Ok(performer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PerformerDto>> Update(int id, [FromBody] UpdatePerformerDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdatePerformer.Command(id, dto), cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeletePerformer.Command(id), cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}