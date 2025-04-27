using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

namespace music_api.Services.Performers.Queries;

public static class GetPerformerById
{
    public record Query(int Id) : IRequest<PerformerDto?>;
    
    public class Handler : IRequestHandler<Query, PerformerDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PerformerDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var performer = await _context.Performers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            
            if (performer is null)
            {
                throw new PerformerValidationException([
                    new ValidationError("Id", "Виконавець не знайдений.")
                ]);
            }
            
            return _mapper.Map<PerformerDto>(performer);
        }
    }
}
