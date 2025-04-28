using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Playlists.Queries;

public static class GetPlaylistById
{
    public record Query(int Id, int? UserId = null) : IRequest<PlaylistDto?>;
    
    public class Handler : IRequestHandler<Query, PlaylistDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (playlist is null)
            {
                return null;
            }

            if (playlist.IsPublic)
            {
                return _mapper.Map<PlaylistDto>(playlist);
            }
            
            if (request.UserId is null || playlist.UserId != request.UserId)
            {
                throw new PlaylistValidationException([
                    new ValidationError("Id", "Плейлист приватний")
                ]);
            }
            
            return _mapper.Map<PlaylistDto>(playlist);
        }
    }
}
