using System.Collections.Generic;

namespace music_api.DTOs;

public class LoginResult
{
    public bool Success { get; set; }
    public UserDto? User { get; set; }
    public string? Token { get; set; }
}

public class RegisterResult
{
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public UserDto? User { get; set; }
    public string? Token { get; set; }
}
