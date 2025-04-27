using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using music_api.DTOs;
using music_api.Services.Playlists.Commands;
using music_api.Services.Playlists.Queries;

namespace music_api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PlaylistController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public PlaylistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<PlaylistDto>> Add([FromBody] CreatePlaylistDto dto, CancellationToken cancellationToken)
    {
        if (!dto.IsPublic && (User.Identity is null || !User.Identity.IsAuthenticated))
        {
            // Только авторизованный может создавать приватный плейлист
            return Unauthorized();
        }
        
        var result = await _mediator.Send(new AddPlaylist.Command(dto, User), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetAll(CancellationToken cancellationToken, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        var result = await _mediator.Send(new GetAllPlaylists.Query(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlaylistDto>> GetById(int id, CancellationToken cancellationToken)
    {
        int? userId = null;
        if (User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var parsedId))
                userId = parsedId;
        }
        var playlist = await _mediator.Send(new GetPlaylistById.Query(id, userId), cancellationToken);

        if (playlist is null)
        {
            return NotFound();
        }
        
        return Ok(playlist);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlaylistDto>> Update(int id, [FromBody] UpdatePlaylistDto dto, CancellationToken cancellationToken)
    {
        // Получаем плейлист для проверки
        var playlist = await _mediator.Send(new GetPlaylistById.Query(id, null), cancellationToken);
        if (playlist == null)
            return NotFound();

        // Неавторизованный может работать только с ничейными плейлистами
        if (User.Identity is null || !User.Identity.IsAuthenticated)
        {
            if (playlist.UserId != null)
            {
                return Unauthorized();
            }
            // Неавторизованный не может делать ничейный плейлист приватным
            if (!dto.IsPublic)
            {
                return Unauthorized();
            }
        }
        else // авторизованный
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || playlist.UserId != userId)
            {
                return Unauthorized();
            }
        }

        var updated = await _mediator.Send(new UpdatePlaylist.Command(id, dto), cancellationToken);
        if (updated is null)
        {
            return NotFound();
        }
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeletePlaylist.Command(id), cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }

    [HttpPost("{playlistId}/add-song")]
    public async Task<ActionResult<PlaylistDto>> AddSongToPlaylist(int playlistId, int songId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddSongToPlaylist.Command(playlistId, songId), cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetMyPlaylists(CancellationToken cancellationToken, [FromQuery] int? page = null, [FromQuery] int? pageSize = null)
    {
        // Если пользователь авторизован — возвращаем его плейлисты
        if (User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();
            var userId = int.Parse(userIdClaim);
            var result = await _mediator.Send(new GetMyPlaylists.Query(userId, page, pageSize), cancellationToken);
            return Ok(result);
        }
        // Если не авторизован — возвращаем публичные плейлисты
        else
        {
            var result = await _mediator.Send(new GetPublicPlaylists.Query(page, pageSize), cancellationToken);
            return Ok(result);
        }
    }

    [HttpGet("random")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetRandomPlaylist([FromQuery] int count, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRandomPlaylist.Query( count), cancellationToken);
        
        return Ok(result);
    }
}