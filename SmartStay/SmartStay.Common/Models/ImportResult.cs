/// <copyright file="ImportResult.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="ImportResult"/> class used in the SmartStay
/// application to summarize the outcome of an import operation for accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>01/12/2024</date>

/// <summary>
/// This namespace contains common models used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Models
{
/// <summary>
/// Represents the result of an accommodation import operation, summarizing the outcome of the process.
/// </summary>
public class ImportResult
{
    /// <summary>
    /// Gets or sets the number of accommodations successfully imported.
    /// </summary>
    public int ImportedCount { get; set; }

    /// <summary>
    /// Gets or sets the number of accommodations that were replaced because they already existed in the collection.
    /// </summary>
    public int ReplacedCount { get; set; }

    /// <summary>
    /// Gets the total number of accommodations processed during the import operation.
    /// This is the sum of <see cref="ImportedCount"/> and <see cref="ReplacedCount"/>.
    /// </summary>
    public int TotalCount => ImportedCount + ReplacedCount;

    /// <summary>
    /// Returns a string representation of the import result, including the number of imported, replaced,
    /// and total accommodations processed.
    /// </summary>
    /// <returns>
    /// A string summarizing the import result.
    /// </returns>
    public override string ToString()
    {
        return $"Imported: {ImportedCount}, Replaced: {ReplacedCount}, Total: {TotalCount}";
    }
}
}
