using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using AutoMapper;

namespace music_api.Services.Songs.Queries;

public static class GetAllSongs
{
    public record Query(int Page, int PageSize, int? PerformerId, int? GenreId) : IRequest<IEnumerable<SongDto>>;
    
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
            
            var songs = await query
                .OrderBy(x => x.Title)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<SongDto>>(songs);
        }
    }
}
