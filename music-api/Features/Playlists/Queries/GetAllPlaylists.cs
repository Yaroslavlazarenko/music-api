using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Playlist;

namespace music_api.Features.Playlists.Queries;

public static class GetAllPlaylists
{
    public record Query(GetAllPlaylistRequestDto Request, int? UserId = null) : IRequest<IEnumerable<PlaylistDto>>;
    
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
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                    .ThenInclude(s => s.Performer)
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                    .ThenInclude(s => s.Genre)
                .AsQueryable();
            
            if (request.UserId.HasValue)
            {
                var userId = request.UserId.Value;
                query = query.Where(p => p.IsPublic || (p.UserId != null && p.UserId == userId));
            }
            else
            {
                query = query.Where(p => p.IsPublic);
            }
            
            if (request.Request is { Page: > 0, PageSize: > 0 })
            {
                query = query.OrderBy(x => x.Title)
                    .Skip((request.Request.Page.Value - 1) * request.Request.PageSize.Value)
                    .Take(request.Request.PageSize.Value);
            }
            else
            {
                query = query.OrderBy(x => x.Title);
            }
            var playlists = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }
    }
}
