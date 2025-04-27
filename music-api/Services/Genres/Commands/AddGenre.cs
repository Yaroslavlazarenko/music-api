using AutoMapper;
using MediatR;
using music_api.DTOs;
using music_api.Entities;
using music_api.Contexts;
using FluentValidation;
using music_api.Validators;
using Microsoft.EntityFrameworkCore;
using music_api.Exceptions;

namespace music_api.Services.Genres.Commands;

public static class AddGenre
{
    public record Command(CreateGenreDto Dto) : IRequest<GenreDto>;
    
    public class Handler : IRequestHandler<Command, GenreDto>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<GenreDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(request.Dto);
            
            var validator = new GenreValidator();
            var validationResult = await validator.ValidateAsync(genre, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var exists = await _context.Genres.AnyAsync(g => g.Name == genre.Name, cancellationToken);
            if (exists)
            {
                throw new GenreValidationException([
                    new ValidationError("Name", $"Жанр з назвою '{genre.Name}' вже існує.")
                ]);
            }
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync(cancellationToken);
            var resultDto = _mapper.Map<GenreDto>(genre);
            return resultDto;
        }
    }
}
