using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Performer;
using music_api.Exceptions;

namespace music_api.Features.Performers.Queries;

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
                throw new NotFoundException("Виконавця з вказаним Id не знайдено");
            }
            
            return _mapper.Map<PerformerDto>(performer);
        }
    }
}
