using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs.Genre;
using music_api.Features.Genres.Commands;
using music_api.Features.Genres.Queries;

namespace music_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class GenreController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public GenreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<GenreDto>> Add([FromBody] CreateGenreDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddGenre.Command(dto), cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll([FromQuery] GetAllGenreRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllGenres.Query(request), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var genre = await _mediator.Send(new GetGenreById.Query(id), cancellationToken);
        
        return Ok(genre);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenreDto>> Update(int id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateGenre.Command(id, dto), cancellationToken);
        
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteGenre.Command(id), cancellationToken);
        
        return NoContent();
    }
}
