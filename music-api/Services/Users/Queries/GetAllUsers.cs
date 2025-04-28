using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Entities;

namespace music_api.Services.Users.Queries;

public static class GetAllUsers
{
    public record Query(int? Page = null, int? PageSize = null) : IRequest<IEnumerable<UserDto>>;
    
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
            
            List<User> users;
            if (request is { Page: > 0, PageSize: > 0 })
            {
                users = await query
                    .OrderBy(x => x.UserName)
                    .Skip((request.Page.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                users = await query.OrderBy(x => x.UserName).ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
