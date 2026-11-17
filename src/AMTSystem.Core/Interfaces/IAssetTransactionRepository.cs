using System;
using System.Collections.Generic;
using System.Text;

using AMTSystem.Core.Requests;

namespace AMTSystem.Core.Interfaces;

public class OpenAssetInfo
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = "";
    public string HeldBy { get; set; } = "";
}

public interface IAssetTransactionRepository
{
    Task<List<OpenAssetInfo>> GetOpenAssetIdsAsync(IEnumerable<int> assetIds);

    Task<int> CreateBatchAsync(IssueAssetsRequest request);
}
