using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs.Performer;
using music_api.Features.Performers.Commands;
using music_api.Features.Performers.Queries;

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
    public async Task<ActionResult<IEnumerable<PerformerDto>>> GetAll([FromQuery] GetAllPerformersRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllPerformers.Query(request), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PerformerDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var performer = await _mediator.Send(new GetPerformerById.Query(id), cancellationToken);
        
        return Ok(performer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PerformerDto>> Update(int id, [FromBody] UpdatePerformerDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdatePerformer.Command(id, dto), cancellationToken);
       
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePerformer.Command(id), cancellationToken);
        
        return NoContent();
    }
}