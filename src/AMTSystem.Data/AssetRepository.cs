using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using AMTSystem.Core.Models;

using MySqlConnector;

namespace AMTSystem.Data;

public class AssetRepository
{
    private const string ConnectionString = "Server=localhost;Port=3306;Database=amtsystem;User ID=root;Password=";
    public void Insert(AssetDraft draft)
    {
        //using var conn = new MySqlConnection(ConnectionString);
        //conn.Open();

        //using var cmd = conn.CreateCommand();
        //cmd.CommandText =
        //    "INSERT INTO assets (name, tag, category_id, status_id, condition_id) " + 
        //    "VALUES (@name, @tag, @category_id, @status_id, @condition_id)";
        //cmd.Parameters.AddWithValue("@name", draft.Name);
        //incomplete
    }

    public static List<Asset> GetAll()
    {
        List<Asset> assets= new List<Asset>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        //cmd.CommandText =
        //    "SELECT * FROM assets";

        cmd.CommandText =
             "WITH RECURSIVE cat_path AS (" +
                 "SELECT id, category AS full_path " +
                 "FROM asset_categories " +
                 "WHERE parent_category_id IS NULL " +

                 "UNION ALL " +

                 "SELECT c.id, CONCAT(cp.full_path, ' > ', c.category) " +
                 "FROM asset_categories c " +
                 "JOIN cat_path cp ON c.parent_category_id = cp.id " +
             ") " + 

             "SELECT a.id, a.name, a.tag, a.serial_number, stat.status, a.category_id, " +
             "   cp.full_path AS category_name, a.components, ac.condition, a.manufacturer, a.model " +
             "FROM assets a " +
             "LEFT JOIN cat_path cp ON cp.id = a.category_id " +
             "LEFT JOIN asset_statuses stat ON stat.id = a.status_id " + 
             "LEFT JOIN asset_conditions ac ON ac.id = a.condition_id;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read()) 
        {
            Asset a = new Asset();
            a.Id = reader.GetInt32("id");
            a.Name = reader.GetString("name");
            a.Tag = reader.GetString("tag");
            a.SerialNumber = reader.GetString("serial_number");
            a.Category_String = reader.GetString("category_name");
            a.Condition_String = reader.GetString("condition");
            a.Status_String = reader.GetString("status");
            a.Manufacturer = reader.GetString("manufacturer");
            a.Model = reader.GetString("model");
            // checks first if the record's attribute is null; if so, attribute is null, else get the value
            a.Components = reader.IsDBNull("components") ? null : reader.GetString("components");
            assets.Add(a);
        }
        return assets;
    }

    public static List<LookupItem> GetStatuses()
    {
        return GetLookup("SELECT id, status AS name FROM asset_statuses ORDER BY status");
    }

    public static List<LookupItem> GetCategories()
    {
        // full path ("Electronics > Laptop") so the dropdown shows the hierarchy
        return GetLookup(@"
        WITH RECURSIVE cat_path AS (
            SELECT id, category AS full_path
            FROM asset_categories
            WHERE parent_category_id IS NULL
            UNION ALL
            SELECT c.id, CONCAT(cp.full_path, ' > ', c.category)
            FROM asset_categories c
            JOIN cat_path cp ON c.parent_category_id = cp.id
        )
        SELECT id, full_path AS name FROM cat_path ORDER BY full_path");
    }

    public static List<LookupItem> GetConditions()
    {
        return GetLookup("SELECT ac.id, ac.condition AS name FROM asset_conditions ac ORDER BY ac.condition");
    }

    public static List<string> GetManufacturers()
    {
        List<string> items = new List<string>();
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT DISTINCT manufacturer FROM assets " +
                          "WHERE manufacturer IS NOT NULL AND manufacturer <> '' " +
                          "ORDER BY manufacturer";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            items.Add(reader.GetString(0));
        }
        return items;
    }

    public static List<string> GetModels(string manufacturer)
    {
        List<string> models = new List<string>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT DISTINCT model as name FROM assets " +
            "WHERE model IS NOT NULL AND manufacturer = @m " +
            "ORDER by model";
        cmd.Parameters.AddWithValue("@m", manufacturer);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            models.Add(reader.GetString(0));
        }
        return models;
    }

    // returns the records in the lookup tables in the database
    private static List<LookupItem> GetLookup(string sql)
    {
        List<LookupItem> items = new List<LookupItem>();

        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            LookupItem item = new LookupItem();
            item.Id = reader.GetInt32("id");
            item.Name = reader.GetString("name");
            items.Add(item);
        }
        return items;
    }


}
