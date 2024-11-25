using SmartStay.IO.FileOperations;

/// <copyright file="PathValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="PathValidator"/> class,
/// validating file paths and file extensions in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>25/11/2024</date>

/// <summary>
/// The <c>SmartStay.IO.Tests.FileOperations</c> namespace contains unit tests for file operations
/// used within the SmartStay application.
/// </summary>
namespace SmartStay.IO.Tests.FileOperations
{
using System;
using System.IO;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="PathValidator"/> class.
/// </summary>
public class PathValidatorTests
{
    /// <summary>
    /// Tests the <see cref="PathValidator.FileExists(string)"/> method to ensure it returns false
    /// for a non-existent file path.
    /// </summary>
    [Fact]
    public void FileExists_NonExistentFile_ReturnsFalse()
    {
        // Arrange
        var filePath = "nonexistent.txt";

        // Act
        var result = PathValidator.FileExists(filePath);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="PathValidator.FileExists(string)"/> method to ensure it returns true
    /// for an existing file path.
    /// </summary>
    [Fact]
    public void FileExists_ExistingFile_ReturnsTrue()
    {
        // Arrange
        var filePath = "testfile.txt";
        File.WriteAllText(filePath, "Sample content");

        try
        {
            // Act
            var result = PathValidator.FileExists(filePath);

            // Assert
            Assert.True(result);
        }
        finally
        {
            // Cleanup
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Tests the <see cref="PathValidator.IsValidFileType(string, string)"/> method to ensure it
    /// throws an exception when the file path is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void IsValidFileType_NullOrEmptyFilePath_ThrowsArgumentException(string filePath)
    {
        // Arrange
        var extension = ".txt";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => PathValidator.IsValidFileType(filePath, extension));
        Assert.Equal("File path cannot be null or empty.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="PathValidator.IsValidFileType(string, string)"/> method to ensure it
    /// returns true for a file path with a matching extension.
    /// </summary>
    [Fact]
    public void IsValidFileType_ValidExtension_ReturnsTrue()
    {
        // Arrange
        var filePath = "testfile.txt";
        var extension = ".txt";

        // Act
        var result = PathValidator.IsValidFileType(filePath, extension);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="PathValidator.IsValidFileType(string, string)"/> method to ensure it
    /// returns false for a file path with a non-matching extension.
    /// </summary>
    [Fact]
    public void IsValidFileType_InvalidExtension_ReturnsFalse()
    {
        // Arrange
        var filePath = "testfile.jpg";
        var extension = ".txt";

        // Act
        var result = PathValidator.IsValidFileType(filePath, extension);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests the <see cref="PathValidator.IsValidFileType(string, string)"/> method to ensure it
    /// performs a case-insensitive comparison of file extensions.
    /// </summary>
    [Fact]
    public void IsValidFileType_CaseInsensitiveExtensionComparison_ReturnsTrue()
    {
        // Arrange
        var filePath = "testfile.TXT";
        var extension = ".txt";

        // Act
        var result = PathValidator.IsValidFileType(filePath, extension);

        // Assert
        Assert.True(result);
    }
}
}
