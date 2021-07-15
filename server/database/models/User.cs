using System;
using server.database.enums;
using server.database.interfaces;

namespace server.database.models
{
  public class User : IBaseEntity
  {
    // Key
    public int Id { get; set; }

    // Properties
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public string FullName { get; set; }
    public UserRole Role { get; set; }
    public UserStatus Status { get; set; }

    // Timestamps
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
  }
}