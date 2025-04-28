using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Performer;
using music_api.Entities;

namespace music_api.Features.Performers.Queries;

public static class GetAllPerformers
{
    public record Query(GetAllPerformersRequestDto Request) : IRequest<IEnumerable<PerformerDto>>;
    
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
            if (request.Request.Page is null || request.Request.PageSize is null || request.Request.Page <= 0 || request.Request.PageSize <= 0)
            {
                performers = await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
            }
            else
            {
                var skip = (request.Request.Page.Value - 1) * request.Request.PageSize.Value;
                performers = await query
                    .OrderBy(x => x.Name)
                    .Skip(skip)
                    .Take(request.Request.PageSize.Value)
                    .ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<PerformerDto>>(performers);
        }
    }
}
