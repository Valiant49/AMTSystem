using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

using AMTSystem.Core.Models;

using MySqlConnector;

namespace AMTSystem.Data;

public class PersonnelRepository
{
    private const string ConnectionString = "Server=localhost;Port=3306;Database=amtsystem;User ID=root;Password=";

    public static List<Department> GetDepartments()
    {
        List<Department> list = new List<Department>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT id, department FROM departments ORDER BY department";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Department d = new Department();
            d.Id = reader.GetInt32("id");
            d.Department_Name = reader.GetString("department");
            list.Add(d);
        }
        return list;
    }

    public static long Insert_Department(string department)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "INSERT into departments (department) VALUES (@department)";

        cmd.Parameters.AddWithValue("@department", department);

        cmd.ExecuteNonQuery();
        return cmd.LastInsertedId;
    }
}
