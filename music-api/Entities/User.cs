using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace music_api.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; }
        public required ICollection<Playlist> Playlists { get; set; }
    }
}