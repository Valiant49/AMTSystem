using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AMTSystem.Core;


public enum Permission { 
    ViewUsers, CreateUser, EditUser, DeleteUser 
}

public static class Gate
{
    private static readonly Dictionary<string, HashSet<Permission>> Map = new()
    {
        //keys must match the values in the DB
        ["Administrator"] = [Permission.ViewUsers, Permission.CreateUser, Permission.EditUser, Permission.DeleteUser],
        ["Technician"] = [Permission.ViewUsers],
    };

    public static bool Allows(Permission p) =>
        Session.CurrentUser is { } u
        && Map.TryGetValue(u.Role, out var perms)
        && perms.Contains(p);

    public static void Authorize(Permission p)
    {
        if (!Allows(p)) throw new UnauthorizedAccessException($"Missing permission: {p}");
    }
}
