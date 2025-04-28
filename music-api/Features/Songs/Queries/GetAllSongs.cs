using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Song;

namespace music_api.Features.Songs.Queries;

public static class GetAllSongs
{
    public record Query(GetAllSongsRequestDto Request) : IRequest<IEnumerable<SongDto>>;
    
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

            if (request.Request.PerformerId.HasValue)
            {
                query = query
                    .Where(x => x.PerformerId == request.Request.PerformerId);
            }

            if (request.Request.GenreId.HasValue)
            {
                query = query
                    .Where(x => x.GenreId == request.Request.GenreId);
            }

            if (request.Request.Page.HasValue && request.Request.PageSize.HasValue && request.Request.Page > 0 && request.Request.PageSize > 0)
            {
                query = query
                    .OrderBy(x => x.Title)
                    .Skip((request.Request.Page.Value - 1) * request.Request.PageSize.Value)
                    .Take(request.Request.PageSize.Value);
            }
            else
            {
                query = query.OrderBy(x => x.Title);
            }
            var songs = await query.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SongDto>>(songs);
        }
    }
}
