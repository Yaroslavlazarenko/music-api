using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Entities;

namespace music_api.Services.Performers.Queries;

public static class GetAllPerformers
{
    public record Query(int Page, int PageSize) : IRequest<IEnumerable<PerformerDto>>;
    
    public class Handler : IRequestHandler<Query, IEnumerable<PerformerDto>>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PerformerDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Performers.AsQueryable();
            List<Performer> performers;
            if (request.Page <= 0 || request.PageSize <= 0)
            {
                performers = await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
            }
            else
            {
                performers = await query
                    .OrderBy(x => x.Name)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<PerformerDto>>(performers);
        }
    }
}
