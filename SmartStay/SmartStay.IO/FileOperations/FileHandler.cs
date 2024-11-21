/// <copyright file="FileHandler.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains utility methods for reading from and writing to files,
/// including directory management for non-existing paths.
/// </file>
/// <summary>
/// Provides file handling operations such as reading from and writing to files.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>20/11/2024</date>

/// <summary>
/// This namespace contains utility methods for file operations used within the SmartStay application.
/// </summary>
namespace SmartStay.IO.FileOperations
{
/// <summary>
/// Provides static methods for file operations such as reading from and writing to files.
/// </summary>
public static class FileHandler
{
    /// <summary>
    /// Reads all text from a file
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    /// <returns>The <see cref="string"/></returns>
    public static string ReadFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return File.ReadAllText(filePath);
    }

    /// <summary>
    /// Writes text to a file. Creates the directory if it doesn't exist
    /// </summary>
    /// <param name="filePath">The filePath<see cref="string"/></param>
    /// <param name="content">The content<see cref="string"/></param>
    public static void WriteFile(string filePath, string content)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.");

        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, content);
    }
}
}
