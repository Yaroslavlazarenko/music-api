using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using music_api.Contexts;
using music_api.DTOs.Playlist;
using music_api.DTOs.Song;
using music_api.Entities;
using music_api.Exceptions;

namespace music_api.Features.Playlists.Commands;

public static class AddSongToPlaylist
{
    public record Command(AddSongToPlaylistRequestDto Request) : IRequest<PlaylistDto?>;

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
                .FirstOrDefaultAsync(p => p.Id == request.Request.PlaylistId, cancellationToken);

            if (playlist is null)
            {
                throw new NotFoundException("Плейлист з вказаним Id не знайдено");
            }
            
            var song = await _context.Songs
                .FindAsync([request.Request.SongId], cancellationToken);

            if (song is null)
            {
                throw new NotFoundException("Пісню з вказаним Id не знайдено");
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
