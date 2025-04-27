using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Genres.Commands;

public static class DeleteGenre
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
            var genre = await _context.Genres.FindAsync([request.Id], cancellationToken);

            if (genre is null)
            {
                throw new GenreValidationException([
                    new ValidationError("Id", "Жанр не знайдено.")
                ]);
            }
            
            _context.Genres.Remove(genre);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
