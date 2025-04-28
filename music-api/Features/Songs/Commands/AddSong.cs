using AutoMapper;
using FluentValidation;
using MediatR;
using music_api.Contexts;
using music_api.DTOs.Song;
using music_api.Entities;
using music_api.Validators;

namespace music_api.Features.Songs.Commands;

public static class AddSong
{
    public record Command(CreateSongDto Dto) : IRequest<SongDto>;
    
    public class Handler : IRequestHandler<Command, SongDto>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SongDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var song = _mapper.Map<Song>(request.Dto);
            
            var validator = new SongValidator();
            var validationResult = await validator.ValidateAsync(song, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            song.CreatedAt = DateTime.UtcNow;
            _context.Songs.Add(song);
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<SongDto>(song);
            return resultDto;
        }
    }
}
