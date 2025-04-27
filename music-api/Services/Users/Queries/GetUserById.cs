using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Users.Queries;

public static class GetUserById
{
    public record Query(int Id) : IRequest<UserDto?>;
    
    public class Handler : IRequestHandler<Query, UserDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<UserDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
           
            if (user is null)
            {
                throw new UserValidationException([
                    new ValidationError("Id", "Користувача не знайдено.")
                ]);
            }
            
            return _mapper.Map<UserDto>(user);
        }
    }
}
