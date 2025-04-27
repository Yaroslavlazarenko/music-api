using MediatR;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Users.Commands;

public static class DeleteUser
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
            var user = await _context.Users
                .FindAsync([request.Id], cancellationToken);

            if (user is null)
            {
                throw new UserValidationException([
                    new ValidationError("Id", "Користувача не знайдено.")
                ]);
            }
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            
            return true;
        }
    }
}
