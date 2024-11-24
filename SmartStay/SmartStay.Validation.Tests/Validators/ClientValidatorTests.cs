/// <copyright file="ClientValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="ClientValidator"/> class,
/// ensuring the correct validation of client-related data in the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>24/11/2024</date>

/// <summary>
/// The <c>SmartStay.Validation.Tests.Validators</c> namespace contains unit tests for the validation logic
/// of different fields.
/// </summary>
namespace SmartStay.Validation.Tests.Validators
{
using SmartStay.Validation;
using SmartStay.Validation.Validators;
using Xunit;

/// <summary>
/// Contains unit tests for the <see cref="ClientValidator"/> class.
/// Tests the validation logic for client-related data in the SmartStay application.
/// </summary>
public class ClientValidatorTests
{
    /// <summary>
    /// Tests the <see cref="ClientValidator.ValidateClientId(int)"/> method to ensure that
    /// it returns the client ID when the ID is valid.
    /// </summary>
    [Fact]
    public void ValidateClientId_ValidId_ReturnsClientId()
    {
        // Arrange
        int validClientId = 123;

        // Act
        var result = ClientValidator.ValidateClientId(validClientId);

        // Assert
        Assert.Equal(validClientId, result);
    }

    /// <summary>
    /// Tests the <see cref="ClientValidator.ValidateClientId(int)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the client ID is invalid.
    /// </summary>
    [Fact]
    public void ValidateClientId_InvalidId_ThrowsValidationException()
    {
        // Arrange
        int invalidClientId = 0; // ID cannot be zero or negative

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => ClientValidator.ValidateClientId(invalidClientId));
        Assert.Equal(ValidationErrorCode.InvalidId, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="ClientValidator.IsValidClientId(int)"/> method to ensure it returns
    /// true when the client ID is valid.
    /// </summary>
    [Fact]
    public void IsValidClientId_ValidId_ReturnsTrue()
    {
        // Arrange
        int validClientId = 456;

        // Act
        var result = ClientValidator.IsValidClientId(validClientId);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="ClientValidator.IsValidClientId(int)"/> method to ensure it returns
    /// false when the client ID is invalid (i.e., non-positive).
    /// </summary>
    [Fact]
    public void IsValidClientId_InvalidId_ReturnsFalse()
    {
        // Arrange
        int invalidClientId = -1; // Invalid ID (negative)

        // Act
        var result = ClientValidator.IsValidClientId(invalidClientId);

        // Assert
        Assert.False(result);
    }
}
}
