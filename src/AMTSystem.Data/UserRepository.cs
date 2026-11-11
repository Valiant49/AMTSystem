using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using AMTSystem.Core.Models;

using MySqlConnector;

namespace AMTSystem.Data;

public class UserRepository
{
    private const string ConnectionString = "Server=localhost;Port=3306;Database=amtsystem;User ID=root;Password=";

    public static List<User> GetUsers()
    {
        List<User> list = new List<User>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT u.id, u.first_name, u.middle_name, u.last_name, u.username, ur.role, u.account_status, t.status, u.created_at, u.updated_at
            FROM users u
            JOIN user_roles ur ON ur.id = u.`role` 
            JOIN account_statuses t ON t.id = u.account_status  
            ORDER BY last_name
            """;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            User u = new User();
            u.Id = reader.GetInt32("id");
            u.FirstName = reader.GetString("first_name");
            u.MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString("middle_name");
            u.LastName = reader.GetString("last_name");
            u.Username = reader.GetString("username");
            u.Role = reader.GetString("role");
            //u.AccountStatus = reader.GetInt32("account_status");
            u.Status = reader.GetString("status");
            list.Add(u);
        }

        return list;
    }
}
