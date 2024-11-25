/// <copyright file="FileHandlerTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="FileHandler"/> class,
/// ensuring proper functionality for reading and writing files, including handling invalid paths.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>25/11/2024</date>

/// <summary>
/// The <c>SmartStay.IO.Tests.FileOperations</c> namespace contains unit tests for file operations
/// used within the SmartStay application.
/// </summary>
namespace SmartStay.IO.Tests.FileOperations
{
using SmartStay.IO.FileOperations;
using System;
using System.IO;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="FileHandler"/> class.
/// </summary>
public class FileHandlerTests
{
    /// <summary>
    /// Tests the <see cref="FileHandler.ReadFile(string)"/> method to ensure it throws an exception
    /// when the file path is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    public void ReadFile_EmptyPath_ThrowsArgumentException(string filePath)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => FileHandler.ReadFile(filePath));
        Assert.Equal("File path cannot be null or empty.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="FileHandler.ReadFile(string)"/> method to ensure it throws a FileNotFoundException
    /// when the file does not exist.
    /// </summary>
    [Fact]
    public void ReadFile_FileDoesNotExist_ThrowsFileNotFoundException()
    {
        // Arrange
        var filePath = "nonexistent.txt";

        // Act & Assert
        var exception = Assert.Throws<FileNotFoundException>(() => FileHandler.ReadFile(filePath));
        Assert.Contains($"File not found: {filePath}", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="FileHandler.ReadFile(string)"/> method to ensure it reads the file content correctly.
    /// </summary>
    [Fact]
    public void ReadFile_ValidFilePath_ReturnsFileContent()
    {
        // Arrange
        var filePath = "test.txt";
        var expectedContent = "Hello, World!";
        File.WriteAllText(filePath, expectedContent);

        try
        {
            // Act
            var content = FileHandler.ReadFile(filePath);

            // Assert
            Assert.Equal(expectedContent, content);
        }
        finally
        {
            // Cleanup
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Tests the <see cref="FileHandler.WriteFile(string, string)"/> method to ensure it throws an exception
    /// when the file path is empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    public void WriteFile_EmptyPath_ThrowsArgumentException(string filePath)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => FileHandler.WriteFile(filePath, "Sample Content"));
        Assert.Equal("File path cannot be null or empty.", exception.Message);
    }

    /// <summary>
    /// Tests the <see cref="FileHandler.WriteFile(string, string)"/> method to ensure it writes content to the file.
    /// </summary>
    [Fact]
    public void WriteFile_ValidFilePath_WritesFileContent()
    {
        // Arrange
        var filePath = "output.txt";
        var content = "Sample Content";

        try
        {
            // Act
            FileHandler.WriteFile(filePath, content);

            // Assert
            Assert.True(File.Exists(filePath));
            Assert.Equal(content, File.ReadAllText(filePath));
        }
        finally
        {
            // Cleanup
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Tests the <see cref="FileHandler.WriteFile(string, string)"/> method to ensure it creates
    /// directories if they do not exist.
    /// </summary>
    [Fact]
    public void WriteFile_NonExistentDirectory_CreatesDirectoryAndWritesFile()
    {
        // Arrange
        var directoryPath = Path.Combine("nonexistent", "subdir");
        var filePath = Path.Combine(directoryPath, "test.txt");
        var content = "Hello, Directory!";

        try
        {
            // Act
            FileHandler.WriteFile(filePath, content);

            // Assert
            Assert.True(Directory.Exists(directoryPath));
            Assert.True(File.Exists(filePath));
            Assert.Equal(content, File.ReadAllText(filePath));
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);
        }
    }
}
}
