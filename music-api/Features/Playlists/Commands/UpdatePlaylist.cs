using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Playlist;
using music_api.Exceptions;

namespace music_api.Features.Playlists.Commands;

public static class UpdatePlaylist
{
    public record Command(int Id, UpdatePlaylistDto Dto) : IRequest<PlaylistDto?>;
    
    public class Handler : IRequestHandler<Command, PlaylistDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (playlist is null)
            {
                throw new NotFoundException("Плейлист з вказаним Id не знайдено");
            }
            
            bool exists;
            if (playlist.UserId != null)
            {
                exists = await _context.Playlists.AnyAsync(p => p.UserId == playlist.UserId && p.Title == request.Dto.Title && p.Id != request.Id, cancellationToken);
            }
            else
            {
                exists = await _context.Playlists.AnyAsync(p => p.UserId == null && p.Title == request.Dto.Title && p.Id != request.Id, cancellationToken);
            }
            if (exists)
            {
                throw new ConflictException($"Плейлист з назвою '{request.Dto.Title}' вже існує.");
            }
            _mapper.Map(request.Dto, playlist);
            
            await _context.SaveChangesAsync(cancellationToken);
            var resultDto = _mapper.Map<PlaylistDto>(playlist);
            return resultDto;
        }
    }
}
