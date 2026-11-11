using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName => string.Join(" ", new[] { FirstName, MiddleName, LastName }
        .Where(s => !string.IsNullOrWhiteSpace(s)
        ));
    public string Username { get; set; }
    private string PasswordHash { get; set; }
    public int RoleNum { get; set; }
    public string Role { get; set; }
    public int AccountStatus { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public static string? GenerateUsername(string firstname, string lastname)
    {
        string username = string.Empty;
        int firstnameLength = 1;
        int lastnameLength = 6;

        if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname)) {
            return string.Empty;
        }

        username = firstname[..firstnameLength] + lastname[..lastnameLength] + Random.Shared.Next(100);

        return username;
    }

    public List<string> Validate()
    {
        List<string> errors = new List<string>();

        if (string.IsNullOrEmpty(FirstName))
            errors.Add("First name is required.");

        if (string.IsNullOrWhiteSpace(LastName))
            errors.Add("Last name is required.");

        if (FirstName.Length > 50)
            errors.Add("First name must be less than 50 characters");

        if (MiddleName?.Length > 50)
            errors.Add("Middle name must be less than 50 characters.");

        if (LastName.Length > 50)
            errors.Add("Last name must be less than 50 characters.");

        return errors;
    }

    public string HashPassword(string input)
    {
        string hashedPassword = input.Trim();

        // continue with hashing + salting logic

        input = hashedPassword;

        return input;
    }
}
