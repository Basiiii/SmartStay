/// <copyright file="FileExtensions.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of extension methods for file-related operations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>20/11/2024</date>

/// <summary>
/// This namespace contains File Extension functions, such as ensuring a directory exists, used within the SmartStay
/// application.
/// </summary>
namespace SmartStay.IO.Extensions
{
/// <summary>
/// Provides extension methods for file-related operations.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Ensures the directory for the given file path exists
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    public static void EnsureDirectoryExists(this string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
}
