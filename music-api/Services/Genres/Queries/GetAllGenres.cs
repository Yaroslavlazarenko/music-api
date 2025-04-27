using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Entities;

namespace music_api.Services.Genres.Queries;

public static class GetAllGenres
{
    public record Query(int Page, int PageSize) : IRequest<IEnumerable<GenreDto>>;
    
    public class Handler : IRequestHandler<Query, IEnumerable<GenreDto>>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<GenreDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Genres.AsQueryable();
            
            List<Genre> genres;
            if (request.Page <= 0 || request.PageSize <= 0)
            {
                genres = await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
            }
            else
            {
                genres = await query
                    .OrderBy(x => x.Name)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }
    }
}
