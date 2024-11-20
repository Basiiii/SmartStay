/// <copyright file="PathValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains utility methods for validating file paths and file extensions.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>20/11/2024</date>

/// <summary>
/// This namespace contains utility methods for file operations used within the SmartStay application.
/// </summary>
namespace SmartStay.IO.FileOperations
{
/// <summary>
/// Provides utility methods for validating file paths and extensions.
/// </summary>
public static class PathValidator
{
    /// <summary>
    /// Checks if the given file path points to an existing file
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// Validates if a file has the specified extension
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    /// <param name="extension">The extension<see cref="string"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public static bool IsValidFileType(string filePath, string extension)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        return Path.GetExtension(filePath).Equals(extension, StringComparison.OrdinalIgnoreCase);
    }
}
}
