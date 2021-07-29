using System;
using System.Collections.Generic;
using System.Linq;
using server.graphql.models;
using server.graphql.types;

namespace server.graphql.extensions
{
  public static class EmployeeQueryExtensions
  {
    /// <summary>
    /// Add employee search to an existing filter root
    /// </summary>
    public static FilterRoot ExtendRootFilter(this FilterRoot filterRoot, string searchValue)
    {
      // Generate the list of filters
      var filters = new List<Filter>();
      if (!string.IsNullOrWhiteSpace(searchValue))
      {
        // Find in Employee Ids
        int searchInEmployeeIds;
        if (Int32.TryParse(searchValue, out searchInEmployeeIds))
        {
          filters.Add(new Filter { Field = "employeeId", Operator = FilterOperator.EQUAL, Value = searchInEmployeeIds });
        }
        // Find in First Names
        filters.Add(new Filter { Field = "firstName", Operator = FilterOperator.CONTAINS, Value = searchValue });
        // Find in Last Names
        filters.Add(new Filter { Field = "lastName", Operator = FilterOperator.CONTAINS, Value = searchValue });
        // Find in Rates
        float searchInRates;
        if (float.TryParse(searchValue, out searchInRates))
        {
          filters.Add(new Filter { Field = "rate", Operator = FilterOperator.EQUAL, Value = searchInRates });
        }
        // Find in Discipline
        var disciplineOptions = GetDisciplineOptions();
        foreach (var disciplineOption in disciplineOptions)
        {
          if (disciplineOption.Value.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
          {
            filters.Add(new Filter { Field = "discipline", Operator = FilterOperator.EQUAL, Value = disciplineOption.Id });
          }
        }
      }

      // Generate an empty root filter if needed
      if (filterRoot == null)
      {
        filterRoot = new FilterRoot
        {
          Logic = FilterLogic.AND,
          Filters = new List<FilterGroup>()
        };
      }

      // Add new filters
      if (filters.Any())
      {
        filterRoot.Filters.Add(new FilterGroup
        {
          Logic = FilterLogic.OR,
          Filters = filters
        });
      }

      return filterRoot.Filters.SelectMany(f => f.Filters).Count() == 0 ? null : filterRoot;
    }

    private static List<FormItemOption> GetDisciplineOptions()
    {
      return new List<FormItemOption>
      {
        new FormItemOption { Id = 1, Value = "PA" },
        new FormItemOption { Id = 2, Value = "PCA" },
        new FormItemOption { Id = 3, Value = "HHA" },
        new FormItemOption { Id = 4, Value = "PCA | HHA" }
      };
    }
  }
}