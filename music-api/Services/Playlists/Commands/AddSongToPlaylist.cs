using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.DTOs;
using music_api.Contexts;
using music_api.Exceptions;

using music_api.Entities;

namespace music_api.Services.Playlists.Commands;

public static class AddSongToPlaylist
{
    public record Command(int PlaylistId, int SongId) : IRequest<PlaylistDto?>;

    public class Handler : IRequestHandler<Command, PlaylistDto?>
    {
        private readonly MusicDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(MusicDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PlaylistDto?> Handle(Command request, CancellationToken cancellationToken)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(p => p.Id == request.PlaylistId, cancellationToken);

            if (playlist is null)
            {
                throw new PlaylistValidationException([
                    new ValidationError("Id", "Плейлист не знайдено.")
                ]);
            }
            
            var song = await _context.Songs
                .FindAsync(new object[] { request.SongId }, cancellationToken);

            if (song is null)
            {
                throw new PlaylistValidationException([
                    new ValidationError("SongId", "Пісню не знайдено.")
                ]);
            }

            if (playlist.PlaylistSongs.All(ps => ps.SongId != song.Id))
            {
                playlist.PlaylistSongs.Add(new PlaylistSong { PlaylistId = playlist.Id, SongId = song.Id });
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<PlaylistDto>(playlist);
        }
    }
}
