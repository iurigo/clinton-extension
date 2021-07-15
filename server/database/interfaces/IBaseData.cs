using System;

namespace server.database.interfaces
{
  public interface IBaseData : IBaseEntity
  {
    // Properties
    byte[] BinaryData { get; set; }
  }
}