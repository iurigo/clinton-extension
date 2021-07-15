using HotChocolate.Types;
using server.database.enums;

namespace server.graphql.mutation.types.employee
{
  public class EmployeeInput
  {
    // Properties
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public EmployeeDiscipline Discipline { get; set; }
    public float Rate { get; set; }
  }


  public class EmployeeInputType : InputObjectType<EmployeeInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<EmployeeInput> descriptor)
    {
      descriptor.Description("The new employee input type.");

      // Properties
      descriptor.Field(f => f.EmployeeId)
        .Name("employeeId")
        .Description("The employee unique identifier.")
        .Type<NonNullType<IntType>>();

      descriptor.Field(f => f.FirstName)
        .Name("firstName")
        .Description("The first name of the employee.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.LastName)
        .Name("lastName")
        .Description("The last name of the employee.")
        .Type<NonNullType<StringType>>();

      descriptor.Field(f => f.Discipline)
        .Name("discipline")
        .Description("The discipline of the employee.")
        .Type<NonNullType<EmployeeDisciplineType>>();

      descriptor.Field(f => f.Rate)
        .Name("rate")
        .Description("The employee's rate.")
        .Type<NonNullType<DecimalType>>();
    }

  }
}