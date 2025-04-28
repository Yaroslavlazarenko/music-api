using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using AutoMapper;
using music_api.Entities;

namespace music_api.Services.Songs.Queries;

public static class GetAllSongs
{
    public record Query(int? Page = null, int? PageSize = null, int? PerformerId = null, int? GenreId = null) : IRequest<IEnumerable<SongDto>>;
    
    public class Handler : IRequestHandler<Query, IEnumerable<SongDto>>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<SongDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Songs
                .Include(x => x.Performer)
                .Include(x => x.Genre)
                .AsQueryable();

            if (request.PerformerId.HasValue)
            {
                query = query
                    .Where(x => x.PerformerId == request.PerformerId);
            }

            if (request.GenreId.HasValue)
            {
                query = query
                    .Where(x => x.GenreId == request.GenreId);
            }

            List<Song> songs;
            if (request is { Page: > 0, PageSize: > 0 })
            {
                songs = await query
                    .OrderBy(x => x.Title)
                    .Skip((request.Page.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                songs = await query.OrderBy(x => x.Title).ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<SongDto>>(songs);
        }
    }
}
