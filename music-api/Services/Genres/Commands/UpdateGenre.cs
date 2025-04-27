using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Genres.Commands;

public static class UpdateGenre
{
    public record Command(int Id, UpdateGenreDto Dto) : IRequest<GenreDto?>;
    
    public class Handler : IRequestHandler<Command, GenreDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GenreDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FindAsync([request.Id], cancellationToken);

            if (genre is null)
            {
                return null;
            }
            
            _mapper.Map(request.Dto, genre);
            
            var exists = await _context.Genres.AnyAsync(g => g.Name == request.Dto.Name && g.Id != request.Id, cancellationToken);
            if (exists)
            {
                throw new GenreValidationException([
                    new ValidationError("Name", $"Жанр з назвою '{request.Dto.Name}' вже існує.")
                ]);
            }
            _mapper.Map(request.Dto, genre);
            await _context.SaveChangesAsync(cancellationToken);
            var resultDto = _mapper.Map<GenreDto>(genre);
            
            return resultDto;
        }
    }
}
