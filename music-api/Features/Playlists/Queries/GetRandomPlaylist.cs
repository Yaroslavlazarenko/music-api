using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Song;

namespace music_api.Features.Playlists.Queries;

public static class GetRandomPlaylist
{
    public record Query(int Count) : IRequest<IEnumerable<SongDto>>;
    
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
            var total = await _context.Songs
                .CountAsync(cancellationToken);

            if (total == 0)
            {
                return new List<SongDto>();
            }
            
            var take = Math.Min(request.Count, total);
            
            var songs = await _context.Songs
                .OrderBy(s => EF.Functions.Random())
                .Take(take)
                .ToListAsync(cancellationToken);
            
            return _mapper.Map<IEnumerable<SongDto>>(songs);
        }
    }
}
