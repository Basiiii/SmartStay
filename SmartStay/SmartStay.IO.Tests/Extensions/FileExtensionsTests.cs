/// <copyright file="FileExtensionsTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="FileExtensions"/> class,
/// ensuring the correct behavior of file-related extension methods in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>25/11/2024</date>

/// <summary>
/// The <c>SmartStay.IO.Tests.Extensions</c> namespace contains unit tests for the extension methods
/// provided for file-related operations in the SmartStay application.
/// </summary>
namespace SmartStay.IO.Tests.Extensions
{
using SmartStay.IO.Extensions;
using System.IO;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="FileExtensions"/> class.
/// Tests the behavior of file-related extension methods.
/// </summary>
public class FileExtensionsTests
{
    /// <summary>
    /// Tests the <see cref="FileExtensions.EnsureDirectoryExists(string)"/> method to ensure that it creates
    /// a new directory when the directory does not exist.
    /// </summary>
    [Fact]
    public void EnsureDirectoryExists_DirectoryDoesNotExist_CreatesDirectory()
    {
        // Arrange
        var testDirectory = Path.Combine(Path.GetTempPath(), "SmartStay", "NonExistentDir");
        var testFilePath = Path.Combine(testDirectory, "testfile.txt");

        try
        {
            // Ensure the directory does not exist before the test
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);

            // Act
            testFilePath.EnsureDirectoryExists();

            // Assert
            Assert.True(Directory.Exists(testDirectory));
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }

    /// <summary>
    /// Tests the <see cref="FileExtensions.EnsureDirectoryExists(string)"/> method to ensure it does not
    /// throw an exception when the directory already exists.
    /// </summary>
    [Fact]
    public void EnsureDirectoryExists_DirectoryExists_DoesNotThrowException()
    {
        // Arrange
        var testDirectory = Path.Combine(Path.GetTempPath(), "SmartStay", "ExistingDir");
        var testFilePath = Path.Combine(testDirectory, "testfile.txt");

        try
        {
            // Ensure the directory exists before the test
            Directory.CreateDirectory(testDirectory);

            // Act
            testFilePath.EnsureDirectoryExists();

            // Assert
            Assert.True(Directory.Exists(testDirectory));
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
        }
    }

    /// <summary>
    /// Tests the <see cref="FileExtensions.EnsureDirectoryExists(string)"/> method to ensure it does not
    /// throw an exception when given an empty file path.
    /// </summary>
    [Theory]
    [InlineData("")]
    public void EnsureDirectoryExists_NullOrEmptyPath_DoesNotThrowException(string filePath)
    {
        // Act & Assert
        var exception = Record.Exception(() => filePath.EnsureDirectoryExists());
        Assert.Null(exception);
    }

    /// <summary>
    /// Tests the <see cref="FileExtensions.EnsureDirectoryExists(string)"/> method to ensure it does not
    /// throw an exception when the file path points to the root directory.
    /// </summary>
    [Fact]
    public void EnsureDirectoryExists_RootDirectoryPath_DoesNotThrowException()
    {
        // Arrange
        var rootDirectory = Path.GetPathRoot(Path.GetTempPath()); // e.g., "C:\"

        // Act & Assert
        if (rootDirectory is not null)
        {
            var exception = Record.Exception(() => rootDirectory.EnsureDirectoryExists());
            Assert.Null(exception);
        }
        else
        {
            throw new InvalidOperationException("Root directory path is null, which is unexpected.");
        }
    }
}
}
