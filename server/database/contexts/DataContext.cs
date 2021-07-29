using Microsoft.EntityFrameworkCore;
using server.database.models;

namespace server.database.contexts
{
  public class DataContext : DbContext
  {
    // Models
    public DbSet<User> Users { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }
    public DbSet<SystemSettings> SystemSettings { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // RefreshToken
      builder.Entity<RefreshToken>(o =>
      {
        o.ToTable("clinton-extension.refresh-tokens");
        o.HasKey(k => new { k.UserId, k.Token });
        o.Property(p => p.Token).HasMaxLength(36);

        o.HasQueryFilter(r => r.User != null);
      });

      // User
      builder.Entity<User>(user =>
      {
        user.ToTable("clinton-extension.users");
        user.Property(p => p.Username).IsRequired().HasMaxLength(32);
        user.Property(p => p.PasswordHash).IsRequired();
        user.Property(p => p.PasswordSalt).IsRequired();
        user.Property(p => p.FullName).IsRequired().HasMaxLength(64);
        user.Property(p => p.Role).IsRequired();
        user.Property(p => p.Status).IsRequired();

        user.HasQueryFilter(f => f.DeletedAt == null);
      });

      // System logs
      builder.Entity<SystemLog>(log =>
      {
        log.ToTable("clinton-extension.system-logs");
      });

      // SystemSettings
      builder.Entity<SystemSettings>(o =>
      {
        o.ToTable("clinton-extension.system-settings");
        o.Property(p => p.Key).IsRequired().HasMaxLength(128);
        o.HasQueryFilter(f => f.DeletedAt == null);
      });

      // Student
      builder.Entity<Employee>(c =>
      {
        c.ToTable("clinton-extension.employees");
        c.Property(p => p.FirstName).IsRequired().HasMaxLength(128);
        c.Property(p => p.LastName).IsRequired().HasMaxLength(128);
        c.Property(p => p.EmployeeId).IsRequired();
        c.Property(p => p.Discipline).IsRequired();
        c.Property(p => p.Rate).IsRequired();
        c.Property(p => p.IsActive).IsRequired();
        
        c.HasQueryFilter(f => f.DeletedAt == null);
      });

      // Seed data
      builder.Seed();
    }

  }
}