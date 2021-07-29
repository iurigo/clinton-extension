using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using server.graphql.mutation.types.user;
using server.database.models;
using server.database.contexts;
using server.extensions;
using server.database.sources;
using server.database.enums;

namespace server.graphql.mutation.modifiers
{
  public interface IUserModifiers
  {
    Task<bool> CreateUser(UserCreateInput input);
    Task<bool> UpdateUser(UserUpdateInput input);
    Task<bool> DeleteUser(int id);
    Task<bool> SetPassword(int id, string password);
  }


  public class UserModifiers : IUserModifiers
  {
    private readonly DataContext _db;
    private readonly ISystemLogModifiers _logs;


    public UserModifiers(DataContext db, ISystemLogModifiers logs)
    {
      this._db = db;
      this._logs = logs;
    }


    /// <summary>
    /// Creates a new user
    /// </summary>
    public async Task<bool> CreateUser(UserCreateInput input)
    {
      using (var transaction = this._db.Database.BeginTransaction())
      {
        try
        {
          // Get the current date
          var now = DateTimeOffset.Now;

          // Check for the duplicates
          var duplicate = await this._db.Users.AnyAsync(i => i.Username == input.Username);
          if (duplicate) { throw new QueryException("A user with the same name already exists."); }

          // Check for the password restrictions
          if (string.IsNullOrEmpty(input.Password) || input.Password.Length < 3)
          {
            throw new QueryException("The password should be at least 3 characters.");
          }

          // Create a new user
          var hashResult = Converters.Hash(input.Password);
          var user = new User
          {
            Username = input.Username,
            PasswordHash = hashResult.hash,
            PasswordSalt = hashResult.salt,
            FullName = input.FullName,
            Role = input.Role,
            Status = input.Status,
            CreatedAt = now,
            UpdatedAt = now
          };

          // Add user to database and save changes
          this._db.Users.Add(user);
          await this._db.SaveChangesAsync();

          // Add system log record
          this._logs.Write(source: EventSource.USER,
            type: EventType.CREATE,
            details: new
            {
              id = user.Id,
              username = input.Username,
              fullName = input.FullName,
              role = input.Role,
              status = input.Status
            },
            date: now
          );

          // Save all changes
          await this._db.SaveChangesAsync();

          // Commit the transition
          transaction.Commit();

          // Return new user
          return true;
        }
        catch
        {
          transaction.Rollback();
          throw;
        }
      }
    }


    /// <summary>
    /// Updates the existing user
    /// </summary>
    public async Task<bool> UpdateUser(UserUpdateInput input)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing user
      var existingUser = await this._db.Users.FirstOrDefaultAsync(i => i.Id == input.Id);
      if (existingUser == null) { throw new QueryException("A user was not found."); }

      // Check for the duplicates
      var duplicate = await this._db.Users.AnyAsync(i => i.Id != input.Id && i.Username == input.Username);
      if (duplicate) { throw new QueryException("A user with the same username already exists."); }

      // Compare existing model with the new one
      var changes = (new ObjectComparer()).CompareObjects(existingUser, input, new string[]
      {
        nameof(User.Username),
        nameof(User.FullName),
        nameof(User.Role),
        nameof(User.Status)
      });

      // Write to system log and save changes if there are any changes
      if (changes.Any())
      {
        // Set property values
        existingUser.Username = input.Username;
        existingUser.FullName = input.FullName;
        existingUser.Role = input.Role;
        existingUser.Status = input.Status;
        existingUser.UpdatedAt = now;

        // Write the log
        this._logs.Write(
          source: EventSource.USER,
          type: EventType.UPDATE,
          details: new
          {
            id = existingUser.Id,
            changes = changes
          },
          date: now
        );

        // Save all changes
        await this._db.SaveChangesAsync();
      }

      // Return success
      return true;
    }


    /// <summary>
    /// Deletes the user
    /// </summary>
    public async Task<bool> DeleteUser(int id)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Get an existing user
      var user = await this._db.Users.FirstOrDefaultAsync(i => i.Id == id);

      // Validate
      if (user == null) { throw new QueryException("The user was not found."); }

      // Delete the user
      user.UpdatedAt = now;
      user.DeletedAt = now;

      // Write to system log
      this._logs.Write(
        source: EventSource.USER,
        type: EventType.DELETE,
        details: new { id = id },
        date: now
      );

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }


    /// <summary>
    /// Set user's password
    /// </summary>
    public async Task<bool> SetPassword(int id, string password)
    {
      // Get the current date
      var now = DateTimeOffset.Now;

      // Check for the password restrictions
      if (string.IsNullOrEmpty(password) || password.Length < 3)
      {
        throw new QueryException("The password should be at least 3 characters.");
      }

      // Get an existing user
      var user = await this._db.Users.FirstOrDefaultAsync(i => i.Id == id);
      if (user == null) { throw new QueryException("A user was not found."); }


      // Set the password
      var hashResult = Converters.Hash(password);
      user.PasswordHash = hashResult.hash;
      user.PasswordSalt = hashResult.salt;
      user.UpdatedAt = now;

      // Add system log record
      this._logs.Write(
        source: EventSource.USER,
        type: EventType.OTHER,
        details: new
        {
          id = user.Id,
          details = "set password"
        },
        date: now
      );

      // Save all changes
      await this._db.SaveChangesAsync();

      // Return success
      return true;
    }
  }
}