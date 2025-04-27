using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs;
using music_api.Entities;

namespace music_api.Services.Playlists.Queries;

public static class GetPublicPlaylists
{
    public record Query(int? Page = null, int? PageSize = null) : IRequest<IEnumerable<PlaylistDto>>;
    
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
                .Where(p => p.IsPublic)
                .OrderBy(x => x.Title);
            
            if (request is { Page: > 0, PageSize: > 0 })
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
