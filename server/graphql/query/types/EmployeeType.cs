using HotChocolate.Types;
using server.database.models;

namespace server.graphql.query.types.employee
{
  public class EmployeeType : ObjectType<Employee>
  {
    protected override void Configure(IObjectTypeDescriptor<Employee> descriptor)
    {
      descriptor.Description("The employee type.");

      // Public fields
      descriptor.Field(f => f.Id)
        .Name("id")
        .Description("The unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.EmployeeId)
        .Name("employeeId")
        .Description("The employee's unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.FirstName)
        .Name("firstName")
        .Description("The employee's first name.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.LastName)
        .Name("lastName")
        .Description("The employee's last name.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Discipline)
        .Name("discipline")
        .Description("The employee's discipline.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Rate)
        .Name("rate")
        .Description("The employee's rate.")
        .Type<NonNullType<DecimalType>>();

      descriptor.Field(f => f.IsActive)
        .Name("isActive")
        .Description("The employee's active status.")
        .Type<NonNullType<BooleanType>>();
      
      descriptor.Field(f => f.CreatedAt)
        .Name("createdAt")
        .Description("Shows when the record was created.")
        .Type<NonNullType<DateTimeType>>();

      descriptor.Field(f => f.UpdatedAt)
        .Name("updatedAt")
        .Description("Shows when the record was last updated.")
        .Type<NonNullType<DateTimeType>>();

      // Hidden fields
      descriptor.Field(f => f.DeletedAt).Ignore();
    }

  }
}