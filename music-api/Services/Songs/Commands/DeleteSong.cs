using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Songs.Commands;

public static class DeleteSong
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
            var song = await _context.Songs
                .FindAsync([request.Id], cancellationToken);

            if (song is null)
            {
                throw new SongValidationException([
                    new ValidationError("Id", "Пісню не знайдено.")
                ]);
            }
            
            _context.Songs.Remove(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
