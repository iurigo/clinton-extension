using System;

namespace server.database.interfaces
{
  public interface IBaseEntity
  {
    int Id { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
  }
}