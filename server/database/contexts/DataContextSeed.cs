using System;
using Microsoft.EntityFrameworkCore;
using server.database.enums;
using server.database.models;
using server.database.sources;
using server.extensions;
using server.services;

namespace server.database.contexts
{
  public static class DataContextSeed
  {
    public static void Seed(this ModelBuilder builder)
    {
      var now = DateTimeOffset.Now;

      SeedUser(builder, now);
      SeedSystemLog(builder, now);
    }

    private static void SeedUser(ModelBuilder builder, DateTimeOffset now)
    {
      builder.Entity<User>(o =>
      {
        var hashResult = Converters.Hash("admin");
        o.HasData(new User
        {
          Id = 1,
          Username = "admin",
          PasswordHash = hashResult.hash,
          PasswordSalt = hashResult.salt,
          FullName = "Administrator",
          Status = UserStatus.ACTIVE,
          Role = UserRole.ADMIN,
          CreatedAt = now,
          UpdatedAt = now
        });
      });
    }

    private static void SeedSystemLog(ModelBuilder builder, DateTimeOffset now)
    {
      builder.Entity<SystemLog>(o =>
      {
        o.HasData(new SystemLog
        {
          Id = 1,
          Source = EventSource.SYSTEM,
          Type = EventType.OTHER,
          Date = now
        });
      });
    }

  }
}