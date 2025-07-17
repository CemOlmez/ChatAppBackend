using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserGroup> UserGroups { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
