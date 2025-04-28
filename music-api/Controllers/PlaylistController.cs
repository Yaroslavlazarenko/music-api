using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using music_api.DTOs.Playlist;
using music_api.DTOs.Song;
using music_api.Features.Playlists.Commands;
using music_api.Features.Playlists.Queries;

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
            return Unauthorized();
        }
        
        var result = await _mediator.Send(new AddPlaylist.Command(dto, User), cancellationToken);
        
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetAll([FromQuery] GetAllPlaylistRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllPlaylists.Query(request), cancellationToken);
        
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
            {
                userId = parsedId;
            }
        }
        var playlist = await _mediator.Send(new GetPlaylistById.Query(id, userId), cancellationToken);
        
        return Ok(playlist);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlaylistDto>> Update(int id, [FromBody] UpdatePlaylistDto dto, CancellationToken cancellationToken)
    {
        var playlist = await _mediator.Send(new GetPlaylistById.Query(id), cancellationToken);
        
        if (User.Identity is null || !User.Identity.IsAuthenticated)
        {
            if (playlist!.UserId is not null)
            {
                return Unauthorized();
            }
            if (!dto.IsPublic)
            {
                return Unauthorized();
            }
        }
        else
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId) || playlist!.UserId != userId)
            {
                return Unauthorized();
            }
        }

        var updated = await _mediator.Send(new UpdatePlaylist.Command(id, dto), cancellationToken);
        
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeletePlaylist.Command(id), cancellationToken);
        
        return NoContent();
    }

    [HttpPost("add-song")]
    public async Task<ActionResult<PlaylistDto>> AddSongToPlaylist([FromBody] AddSongToPlaylistRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AddSongToPlaylist.Command(request), cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetMyPlaylists([FromQuery] GetMyPlaylistsRequestDto request, CancellationToken cancellationToken = default)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
            {
                return Unauthorized();
            }
            var userId = int.Parse(userIdClaim);
            var result = await _mediator.Send(new GetMyPlaylists.Query(userId, request), cancellationToken);
            
            return Ok(result);
        }
        else
        {
            var result = await _mediator.Send(new GetPublicPlaylists.Query(request), cancellationToken);
            
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