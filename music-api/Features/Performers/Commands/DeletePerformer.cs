using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Features.Performers.Commands;

public static class DeletePerformer
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
            var performer = await _context.Performers.FindAsync([request.Id], cancellationToken);
            
            if (performer is null)
            {
                throw new NotFoundException("Виконавця з даним Id не знайдено");
            }
            
            _context.Performers.Remove(performer);
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
