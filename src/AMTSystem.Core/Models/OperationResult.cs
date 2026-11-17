using System;
using System.Collections.Generic;
using System.Text;

namespace AMTSystem.Core.Models;

public class OperationResult
{
    public bool Success { get; private set; }
    public List<string> Errors { get; private set; } = new();
    public int? BatchId { get; private set; }

    public static OperationResult Ok(int? batchId = null) =>
        new() { Success = true, BatchId = batchId };

    public static OperationResult Fail(string error) =>
        new() { Success = false, Errors = new List<string> { error } };

    public static OperationResult Fail(IEnumerable<string> errors) =>
        new() { Success = false, Errors = errors.ToList() };
}
