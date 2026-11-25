using AMTSystem.Core.Exceptions;
using AMTSystem.Core.Interfaces;
using AMTSystem.Core.Models;
using AMTSystem.Core.Requests;

namespace AMTSystem.Core.Services;

public class AssetTransactionService
//public class AssetIssuanceService
{
    private readonly IAssetTransactionRepository _txRepo;
    //private readonly AssetIssuanceService _service;

    public AssetTransactionService(IAssetTransactionRepository txRepo)
    {
        _txRepo = txRepo;
    }

    public async Task<OperationResult> IssueAsync(AssetTransactionRequest request)
    {

        var errors = request.Validate();
        if (errors.Count > 0)
            return OperationResult.Fail(errors);

        errors = await ValidateAgainstDbAsync(request);
        if (errors.Count > 0)
            return OperationResult.Fail(errors);

        try
        {
            int batchId = await _txRepo.CreateBatchAsync(request);
            return OperationResult.Ok(batchId);
        } 
        catch (AssetAlreadyIssuedException)
        {
            return OperationResult.Fail(
                "An asset was just issued by someone else. Refresh and try again.");
        }
    }

    public async Task<OperationResult> LendAsync(AssetTransactionRequest request)
    {
        var errors = request.Validate();
        if (errors.Count > 0)
            return OperationResult.Fail(errors);

        errors = await ValidateAgainstDbAsync(request);
        if (errors.Count > 0)
            return OperationResult.Fail(errors);

        try
        {
            int batchId = await _txRepo.CreateBatchAsync(request);
            return OperationResult.Ok(batchId);
        } 
        catch (AssetAlreadyIssuedException)
        {
            return OperationResult.Fail(
                "As asset was just lent by someone else. Refresh and try again.");
        }
    }

    private async Task<List<string>> ValidateAgainstDbAsync(AssetTransactionRequest request)
    {
        var errors = new List<string>();

        var open = await _txRepo.GetOpenAssetIdsAsync(request.Items.Select(i => i.AssetId));
        foreach (var o in open)
            errors.Add($"{o.AssetName} is currently held by {o.HeldBy}.");

        // Add more DB checks here later (personnel active, asset status, etc.)

        return errors;
    }

    public async Task<OperationResult> ReturnAsync(ReturnAssetsRequest request)
    {
        var errors = request.Validate();
        if (errors.Count > 0)
            return OperationResult.Fail(errors);

        try
        {
            await _txRepo.ReturnAsync(request);
            return OperationResult.Ok();
        }
        catch (TransactionAlreadyReturnedException)
        {
            return OperationResult.Fail(
                "An item was already returned by someone else. Reload and try again.");
        }
    }
}
