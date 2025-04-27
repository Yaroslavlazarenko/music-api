using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;

namespace music_api.Services.Users.Queries;

public static class GetAllUsers
{
    public record Query(int Page, int PageSize) : IRequest<IEnumerable<UserDto>>;
    
    public class Handler : IRequestHandler<Query, IEnumerable<UserDto>>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Users.AsQueryable();
            
            var users = await query
                .OrderBy(x => x.UserName)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
