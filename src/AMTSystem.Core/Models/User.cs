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
    public string PasswordHash { get; set; }
    public int RoleId { get; set; }
    public string Role { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    public static string GenerateUsernameBase(string firstName, string lastName)
    {
        var first = new string((firstName ?? "").Where(char.IsLetter).ToArray());
        var last = new string((lastName ?? "").Where(char.IsLetter).ToArray());

        if (first.Length == 0 || last.Length == 0) return string.Empty;

        return (first[..1] + last[..Math.Min(6, last.Length)]).ToLower();
    }

    // on save only
    public static string GenerateUsername(string firstName, string lastName)
    {
        var b = GenerateUsernameBase(firstName, lastName);
        return b.Length == 0 ? string.Empty : b + Random.Shared.Next(100);
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

        if (RoleId == 0)
            errors.Add("Please select a role for this user.");

        return errors;
    }
}
