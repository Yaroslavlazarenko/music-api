using MediatR;
using music_api.Contexts;
using Microsoft.EntityFrameworkCore;
using music_api.Exceptions;

namespace music_api.Services.Playlists.Commands;

public static class DeletePlaylist
{
    public record Command(int Id) : IRequest<bool>;
    
    public class Handler : IRequestHandler<Command, bool>
    {
        private readonly MusicDbContext _context;
        
        public Handler(MusicDbContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .Include(p => p.Songs)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (playlist is null)
            {
                throw new PlaylistValidationException([
                    new ValidationError("Id", "Плейлист не знайдено.")
                ]);
            }
            
            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
