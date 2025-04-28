using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using music_api.DTOs;
using music_api.Services.Songs.Commands;
using music_api.Services.Songs.Queries;

namespace music_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class SongController : ControllerBase
{
    private readonly IMediator _mediator;

    public SongController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SongDto>> Add([FromBody] CreateSongDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddSong.Command(dto), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetAll(int page, int pageSize, int? performerId, int? genreId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllSongs.Query(page, pageSize, performerId, genreId), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SongDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var song = await _mediator.Send(new GetSongById.Query(id), cancellationToken);

        if (song is null)
        {
            return NotFound();
        }
        
        return Ok(song);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SongDto>> Update(int id, [FromBody] UpdateSongDto dto, CancellationToken cancellationToken)
    {
        var updated = await _mediator.Send(new UpdateSong.Command(id, dto), cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteSong.Command(id), cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}