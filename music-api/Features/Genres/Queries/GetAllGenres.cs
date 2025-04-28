using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Genre;
using music_api.Entities;

namespace music_api.Features.Genres.Queries;

public static class GetAllGenres
{
    public record Query(GetAllGenreRequestDto Request) : IRequest<IEnumerable<GenreDto>>;
    
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
            if (request.Request.Page is null || request.Request.PageSize is null || request.Request.Page <= 0 || request.Request.PageSize <= 0)
            {
                genres = await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
            }
            else
            {
                var skip = (request.Request.Page.Value - 1) * request.Request.PageSize.Value;

                genres = await query
                    .OrderBy(x => x.Name)
                    .Skip(skip)
                    .Take(request.Request.PageSize.Value)
                    .ToListAsync(cancellationToken);
            }
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }
    }
}
