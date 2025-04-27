using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Songs.Queries;

public static class GetSongById
{
    public record Query(int Id) : IRequest<SongDto?>;
    
    public class Handler : IRequestHandler<Query, SongDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SongDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs
                .Include(x => x.Performer)
                .Include(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (song is null)
            {
                throw new SongValidationException([
                    new ValidationError("Id", "Пісню не знайдено.")
                ]);
            }
            
            return _mapper.Map<SongDto>(song);
        }
    }
}
