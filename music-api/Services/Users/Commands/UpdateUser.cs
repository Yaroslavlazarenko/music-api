using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Users.Commands;

public static class UpdateUser
{
    public record Command(int Id, UpdateUserDto Dto) : IRequest<UserDto?>;
    
    public class Handler : IRequestHandler<Command, UserDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FindAsync([request.Id], cancellationToken);

            if (user is null)
            {
                throw new UserValidationException([
                    new ValidationError("Id", "Користувача не знайдено.")
                ]);
            }
            
            var exists = await _context.Users.AnyAsync(u => u.Email == request.Dto.Email && u.Id != request.Id, cancellationToken);
            if (exists)
            {
                throw new UserValidationException([
                    new ValidationError("Email", $"Користувач з таким email вже існує.")
                ]);
            }
            _mapper.Map(request.Dto, user);
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<UserDto>(user);
            return resultDto;
        }
    }
}
