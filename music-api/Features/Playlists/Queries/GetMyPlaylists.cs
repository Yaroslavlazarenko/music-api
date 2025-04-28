using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Playlist;

namespace music_api.Features.Playlists.Queries;

public static class GetMyPlaylists
{
    public record Query(int UserId, GetMyPlaylistsRequestDto Request) : IRequest<IEnumerable<PlaylistDto>>;
    
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
                .Where(p => p.UserId == request.UserId)
                .OrderBy(x => x.Title);
            
            if (request.Request is { PageSize: > 0, Page: > 0 })
            {
                var skip = (request.Request.Page.Value - 1) * request.Request.PageSize.Value;
                
                query = query
                    .Skip(skip)
                    .Take(request.Request.PageSize.Value)
                    .OrderBy(x => x.Title);
            }
            
            var playlists = await query.ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }
    }
}
