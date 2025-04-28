using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Features.Songs.Commands;

public static class DeleteSong
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
            var song = await _context.Songs
                .FindAsync([request.Id], cancellationToken);

            if (song is null)
            {
                throw new NotFoundException("Пісню з вказаним Id не знайдено");
            }
            
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
