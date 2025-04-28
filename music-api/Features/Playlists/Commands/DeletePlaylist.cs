using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Features.Playlists.Commands;

public static class DeletePlaylist
{
    public record Command(int Id) : IRequest;
    
    public class Handler : IRequestHandler<Command>
    {
        private readonly MusicDbContext _context;
        
        public Handler(MusicDbContext context)
        {
            _context = context;
        }
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (playlist is null)
            {
                throw new NotFoundException("Плейлист з вказаним Id не знайдено");
            }
            
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
