using HotChocolate.Types;
using server.database.models;
using server.graphql.types;

namespace server.graphql.query.types
{
  public class SystemLogType : ObjectType<SystemLog>
  {
    protected override void Configure(IObjectTypeDescriptor<SystemLog> descriptor)
    {
      descriptor.Description("A system log record.");

      // Properties
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("Unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.Source)
        .Name("source")
        .Description("The event source (affected entity).")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Type)
        .Name("type")
        .Description("The event type.")
        .Type<NonNullType<EventTypeType>>();

      descriptor.Field(f => f.Details)
        .Name("details")
        .Description("The event details.")
        .Type<StringType>();

      descriptor.Field(f => f.Date)
        .Name("date")
        .Description("The event date.")
        .Type<NonNullType<DateTimeType>>();


      descriptor.Field(f => f.UserId).Ignore();
    }

  }
}