using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs;
using music_api.Services.Genres.Commands;
using music_api.Services.Genres.Queries;

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
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAll(int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllGenres.Query(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GenreDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var genre = await _mediator.Send(new GetGenreById.Query(id), cancellationToken);
        if (genre == null) return NotFound();
        return Ok(genre);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<GenreDto>> Update(int id, [FromBody] UpdateGenreDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateGenre.Command(id, dto), cancellationToken);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteGenre.Command(id), cancellationToken);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
