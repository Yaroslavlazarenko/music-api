using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Performers.Commands;

public static class DeletePerformer
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
            var performer = await _context.Performers.FindAsync([request.Id], cancellationToken);
            
            if (performer is null)
            {
                throw new PerformerValidationException([
                    new ValidationError("Id", "Виконавець не знайдений.")
                ]);
            }
            
            _context.Performers.Remove(performer);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
