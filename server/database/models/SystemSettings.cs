using System;
using server.database.interfaces;

namespace server.database.models
{
	public class SystemSettings : IBaseEntity
	{
		// Key
    public int Id { get; set; }

		// Properties
		public string Key { get; set; }
		public string Value { get; set; }

		// TimeStamp
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
	}
}
