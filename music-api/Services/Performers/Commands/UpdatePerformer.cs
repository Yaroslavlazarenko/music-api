using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Performers.Commands;

public static class UpdatePerformer
{
    public record Command(int Id, UpdatePerformerDto Dto) : IRequest<PerformerDto?>;
    
    public class Handler : IRequestHandler<Command, PerformerDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PerformerDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var performer = await _context.Performers.FindAsync([request.Id], cancellationToken);

            if (performer is null)
            {
                return null;
            }
            
            var exists = await _context.Performers.AnyAsync(p => p.Name == request.Dto.Name && p.Id != request.Id, cancellationToken);
            if (exists)
            {
                throw new PerformerValidationException([
                    new ValidationError("Name", $"Виконавець з ім'ям '{request.Dto.Name}' вже існує.")
                ]);
            }
            _mapper.Map(request.Dto, performer);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            var resultDto = _mapper.Map<PerformerDto>(performer);
            
            return resultDto;
        }
    }
}
