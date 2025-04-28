using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using MediatR;
using music_api.Contexts;
using music_api.DTOs.Playlist;
using music_api.Entities;
using music_api.Validators;

namespace music_api.Features.Playlists.Commands;

public static class AddPlaylist
{
    public record Command(CreatePlaylistDto Dto, ClaimsPrincipal? User) : IRequest<PlaylistDto>;
    
    public class Handler : IRequestHandler<Command, PlaylistDto>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var playlist = _mapper.Map<Playlist>(request.Dto);
            var validator = new PlaylistValidator();
            var validationResult = await validator.ValidateAsync(playlist, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            playlist.CreatedAt = DateTime.UtcNow;

            if (request.User?.Identity is null || !request.User.Identity.IsAuthenticated)
            {
                playlist.IsPublic = true;
                playlist.UserId = null;
            }
            else
            {
                var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var userId))
                {
                    playlist.UserId = userId;
                }
                playlist.IsPublic = request.Dto.IsPublic;
            }

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            var resultDto = _mapper.Map<PlaylistDto>(playlist);
            return resultDto;
        }
    }
}
