using AutoMapper;
using MediatR;
using music_api.Contexts;
using music_api.DTOs.Song;
using music_api.Exceptions;

namespace music_api.Features.Songs.Commands;

public static class UpdateSong
{
    public record Command(int Id, UpdateSongDto Dto) : IRequest<SongDto?>;
    
    public class Handler : IRequestHandler<Command, SongDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SongDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var song = await _context.Songs
                .FindAsync([request.Id], cancellationToken);

            if (song is null)
            {
                throw new NotFoundException("Пісню з вказаним Id не знайдено");
            }
            
            _mapper.Map(request.Dto, song);
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<SongDto>(song);
            return resultDto;
        }
    }
}
