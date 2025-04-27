using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Genres.Queries;

public static class GetGenreById
{
    public record Query(int Id) : IRequest<GenreDto?>;
    
    public class Handler : IRequestHandler<Query, GenreDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GenreDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            if (genre is null)
            {
                throw new GenreValidationException([
                    new ValidationError("Id", "Жанр не знайдено.")
                ]);
            }
            
            return _mapper.Map<GenreDto>(genre);
        }
    }
}
