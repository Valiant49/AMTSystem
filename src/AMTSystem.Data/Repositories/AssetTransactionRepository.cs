using System;
using System.Collections.Generic;
using System.Text;

using AMTSystem.Core;

using AMTSystem.Core.Exceptions;
using AMTSystem.Core.Interfaces;
using AMTSystem.Core.Requests;

using MySqlConnector;

namespace AMTSystem.Data.Repositories;

public static class AssetStatusIds
{
    public const int Available = 1;
    public const int Issued = 2;
    public const int Lent = 3;
}

public class AssetTransactionRepository : IAssetTransactionRepository
{
    private const string ConnectionString = "Server=localhost;Port=3306;Database=amtsystem;User ID=root;Password=";

    // transactions
    //public static long IssueAssets()
    //{
    //    using var conn = new MySqlConnection(ConnectionString);
    //    conn.Open();

    //    using var cmd = conn.CreateCommand();
    //    cmd.CommandText =
    //        """

    //        """;

    //    return cmd.LastInsertedId;
    //}

    public async Task<List<OpenAssetInfo>> GetOpenAssetIdsAsync(IEnumerable<int> assetIds)
    {
        var ids = assetIds.ToList();
        var result = new List<OpenAssetInfo>();
        if (ids.Count == 0) return result;

        var paramNames = ids.Select((_, i) => $"@a{i}").ToList();

        var sql = $@"
            SELECT t.asset_id,
                a.name AS asset_name,
                CONCAT(p.first_name, ' ', p.last_name) AS held_by
            FROM asset_transactions t
            JOIN assets a       ON a.id = t.asset_id
            JOIN personnels p   ON p.id = t.personnel_id
            WHERE t.returned_at IS NULL
                AND t.asset_id IN ({string.Join(",", paramNames)});
        ";
        
        await using var conn = new MySqlConnection(ConnectionString);
        await conn.OpenAsync();

        await using var cmd = new MySqlCommand(sql, conn);
        for (int i = 0; i < ids.Count; i++)
        {
            cmd.Parameters.AddWithValue(paramNames[i], ids[i]);
        }

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new OpenAssetInfo
            {
                AssetId = reader.GetInt32("asset_id"),
                AssetName = reader.GetString("asset_name"),
                HeldBy = reader.GetString("held_by")
            });
        }
        return result;
    }

    public async Task<int> CreateBatchAsync(IssueAssetsRequest request)
    {
        await using var conn = new MySqlConnection(ConnectionString);
        await conn.OpenAsync();
        await using var tx = await conn.BeginTransactionAsync();

        try
        {
            // 1. Header
            int batchId;
            await using (var batchCmd = new MySqlCommand(@"
            INSERT INTO asset_transactions_batches (user_id, personnel_id, remarks)
            VALUES (@user, @person, @remarks);", conn, tx))
            {
                batchCmd.Parameters.AddWithValue("@user", request.UserId);
                batchCmd.Parameters.AddWithValue("@person", request.PersonnelId);
                batchCmd.Parameters.AddWithValue("@remarks", (object?)request.Remarks ?? DBNull.Value);
                await batchCmd.ExecuteNonQueryAsync();
                batchId = (int)batchCmd.LastInsertedId;
            }

            // 2. Items
            foreach (var item in request.Items)
            {
                await using var itemCmd = new MySqlCommand(@"
                INSERT INTO asset_transactions
                    (batch_id, user_id, personnel_id, asset_id, transaction_type,
                     asset_condition_at_transaction, remarks)
                VALUES
                    (@batch, @user, @person, @asset, @type, @cond, @remarks);", conn, tx);

                itemCmd.Parameters.AddWithValue("@batch", batchId);
                itemCmd.Parameters.AddWithValue("@user", request.UserId);
                itemCmd.Parameters.AddWithValue("@person", request.PersonnelId);
                itemCmd.Parameters.AddWithValue("@asset", item.AssetId);
                itemCmd.Parameters.AddWithValue("@type", request.Type.ToString());
                itemCmd.Parameters.AddWithValue("@cond", item.ConditionId);
                itemCmd.Parameters.AddWithValue("@remarks", (object?)item.Remarks ?? DBNull.Value);
                await itemCmd.ExecuteNonQueryAsync();
            }

            foreach (var item in request.Items)
            {
                await using var statusCmd = new MySqlCommand(@"
                UPDATE assets
                SET status_id = @newStatus
                WHERE id = @asset AND status_id = @availableStatus;
                ", conn, tx);

                statusCmd.Parameters.AddWithValue("@newStatus", AssetStatusIds.Issued);
                statusCmd.Parameters.AddWithValue("@asset", item.AssetId);
                statusCmd.Parameters.AddWithValue("@availableStatus", AssetStatusIds.Available);

                int affected = await statusCmd.ExecuteNonQueryAsync();
                if (affected == 0)
                    throw new AssetAlreadyIssuedException();
            }


            await tx.CommitAsync();
            return batchId;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            await tx.RollbackAsync();
            throw new AssetAlreadyIssuedException(ex);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
