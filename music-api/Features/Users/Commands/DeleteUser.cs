using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Features.Users.Commands;

public static class DeleteUser
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
            var user = await _context.Users
                .FindAsync([request.Id], cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("Користувача з вказаним Id не знайдено");
            }
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
