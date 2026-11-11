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
            d.DepartmentName = reader.GetString("department");
            list.Add(d);
        }
        return list;
    }

    public static List<Personnel> GetPersonnels()
    {
        List<Personnel> list = new List<Personnel>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = 
        """
        SELECT p.id, p.first_name, p.middle_name, p.last_name,
               p.employee_no, p.account_status, t.status AS status,
               p.department AS department_id,
               d.department AS department_name
        FROM personnels p
        JOIN departments d ON d.id = p.department
        JOIN account_statuses t ON t.id = p.account_status  
        ORDER BY p.last_name
        """;


        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Personnel p = new Personnel(); 
            p.Id = reader.GetInt32("id");
            p.FirstName = reader.GetString("first_name");
            p.MiddleName = reader.IsDBNull(reader.GetOrdinal("middle_name")) ? null : reader.GetString("middle_name");
            p.LastName = reader.GetString("last_name");
            p.AccountStatus = reader.GetInt32("account_status"); 
            p.Status = reader.GetString("status");
            p.DepartmentId = reader.GetInt32("department_id");
            p.Department = reader.GetString("department_name");
            p.EmployeeNo = reader.GetString("employee_no");
            list.Add(p);
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
    public static long Insert_Personnel(Personnel personnel)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "INSERT into personnels (first_name, middle_name, last_name, account_status, department, employee_no)" +
            "VALUES (@first_name, @middle_name, @last_name, @account_status, @department, @employee_no)";

        cmd.Parameters.AddWithValue("@first_name", personnel.FirstName);
        cmd.Parameters.AddWithValue("@middle_name", (object?)personnel.MiddleName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@last_name", personnel.LastName);
        cmd.Parameters.AddWithValue("@account_status", 1);
        cmd.Parameters.AddWithValue("@employee_no", personnel.EmployeeNo);
        cmd.Parameters.AddWithValue("@department", personnel.DepartmentId);

        cmd.ExecuteNonQuery();
        return cmd.LastInsertedId;
    }
}
