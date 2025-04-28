using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Features.Genres.Commands;

public static class DeleteGenre
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
            var genre = await _context.Genres.FindAsync([request.Id], cancellationToken);

            if (genre is null)
            {
                throw new NotFoundException("Жанр з вказаним Id не знайдено.");
            }
            
            _context.Genres.Remove(genre);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
