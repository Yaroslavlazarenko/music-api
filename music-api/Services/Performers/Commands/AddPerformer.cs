using AutoMapper;
using MediatR;
using music_api.DTOs;
using music_api.Entities;
using music_api.Exceptions;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using FluentValidation;
using music_api.Validators;

namespace music_api.Services.Performers.Commands;

public static class AddPerformer
{
    public record Command(CreatePerformerDto Dto) : IRequest<PerformerDto>;
    
    public class Handler : IRequestHandler<Command, PerformerDto>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PerformerDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var performer = _mapper.Map<Performer>(request.Dto);
           
            var validator = new PerformerValidator();
            var validationResult = await validator.ValidateAsync(performer, cancellationToken);
            
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var exists = await _context.Performers.AnyAsync(p => p.Name == performer.Name, cancellationToken);
            if (exists)
            {
                throw new PerformerValidationException([
                    new ValidationError("Name", $"Виконавець з ім'ям '{performer.Name}' вже існує.")
                ]);
            }
            performer.CreatedAt = DateTime.UtcNow;
            
            _context.Performers.Add(performer);
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<PerformerDto>(performer);
            return resultDto;
        }
    }
}
