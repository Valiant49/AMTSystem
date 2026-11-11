using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Models;

public  class Personnel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName => string.Join(" ", new [] {FirstName, MiddleName, LastName}
        .Where(s => !string.IsNullOrWhiteSpace(s)
        ));
    public string EmployeeNo { get; set; }
    public int AccountStatus { get; set; }
    public string Status { get; set; }
    public int DepartmentId { get; set; }
    public string Department { get; set; }

    public List<string> Validate()
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrWhiteSpace(FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(LastName))
            errors.Add("Last name is required.");

        if (FirstName.Length > 50)
            errors.Add("First name must be less than 50 characters");

        if (MiddleName?.Length > 50)
            errors.Add("Middle name must be less than 50 characters.");

        if (LastName.Length > 50)
            errors.Add("Last name must be less than 50 characters.");

        if (string.IsNullOrWhiteSpace(EmployeeNo))
            errors.Add("Employee number is required.");

        return errors;
    }


}
