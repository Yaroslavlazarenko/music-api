using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.User;
using music_api.Exceptions;

namespace music_api.Features.Users.Commands;

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
                throw new NotFoundException("Користувача з вказаним Id не знайдено");
            }
            
            var exists = await _context.Users.AnyAsync(u => u.Email == request.Dto.Email && u.Id != request.Id, cancellationToken);
            if (exists)
            {
                throw new ConflictException("Користувач з таким email вже існує.");
            }
            _mapper.Map(request.Dto, user);
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<UserDto>(user);
            return resultDto;
        }
    }
}
