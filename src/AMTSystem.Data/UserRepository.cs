using System.Security.Cryptography;

using AMTSystem.Core;
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
            u.StatusId = reader.GetInt32("account_status");
            u.Status = reader.GetString("status");
            //u.CreatedAt = reader.GetDateTime("created_at");
            //u.LastUpdatedAt = reader.GetDateTime("updated_at");
            list.Add(u);
        }

        return list;
    }

    public static long AddUser(User user)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
            INSERT INTO USERS (first_name, middle_name, last_name, username, password_hash, role, account_status) 
            VALUES (@first_name, @middle_name, @last_name, @username, @password_hash, @role, @account_status)
            """;

        cmd.Parameters.AddWithValue("@first_name", user.FirstName);
        cmd.Parameters.AddWithValue("@middle_name", user.MiddleName);
        cmd.Parameters.AddWithValue("@last_name", user.LastName);
        cmd.Parameters.AddWithValue("@username", user.Username);
        cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@role", user.RoleId);
        cmd.Parameters.AddWithValue("@account_status", user.StatusId);
        //cmd.Parameters.AddWithValue("@created_at", user.CreatedAt);
        //cmd.Parameters.AddWithValue("@updated_at", user.LastUpdatedAt);

        cmd.ExecuteNonQuery();

        return cmd.LastInsertedId;
    }
    public static List<User> GetRoles()
    {
        List<User> list = new List<User>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, role FROM user_roles ORDER BY role";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            User r = new User();
            r.Id = reader.GetInt32("id");
            r.Role = reader.GetString("role");
            list.Add(r);
        }
        return list;
    }

    public static List<User> GetStatuses()
    {
        var list = new List<User>();
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id, status FROM account_statuses ORDER BY id", conn);
        using var r = cmd.ExecuteReader();

        while (r.Read())
        {
            User u = new User();
            u.Id = r.GetInt32("id");
            u.Status = r.GetString("status");
            list.Add(u);
        }
        return list;
    }

    public static bool UsernameExists(string username)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @u", conn);
        cmd.Parameters.AddWithValue("@u", username);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }
}
