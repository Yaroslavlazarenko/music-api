using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;

namespace music_api.Services.Playlists.Queries;

public static class GetMyPlaylists
{
    public record Query(int UserId, int? Page = null, int? PageSize = null) : IRequest<IEnumerable<PlaylistDto>>;
    
    public class Handler : IRequestHandler<Query, IEnumerable<PlaylistDto>>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PlaylistDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Playlists
                .Include(p => p.User)
                .Include(p => p.Songs)
                .Where(p => p.UserId == request.UserId)
                .OrderBy(x => x.Title);
            
            if (request is { PageSize: > 0, Page: > 0 })
            {
                query = query
                    .Skip((request.Page.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .OrderBy(x => x.Title);
            }
            
            var playlists = await query.ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }
    }
}
