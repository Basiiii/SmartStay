/// <copyright file="AddressValidatorTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for the <see cref="AddressValidator"/> class,
/// ensuring the correct validation of address-related data in the SmartStay application.
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
/// Contains unit tests for the <see cref="AddressValidator"/> class.
/// Tests the validation logic for addresses used in the SmartStay application.
/// </summary>
public class AddressValidatorTests
{
    /// <summary>
    /// Tests the <see cref="AddressValidator.ValidateAddress(string)"/> method to ensure that
    /// it returns the address when the address is valid.
    /// </summary>
    [Fact]
    public void ValidateAddress_ValidAddress_ReturnsAddress()
    {
        // Arrange
        var validAddress = "123 Main Street, Cityville";

        // Act
        var result = AddressValidator.ValidateAddress(validAddress);

        // Assert
        Assert.Equal(validAddress, result);
    }

    /// <summary>
    /// Tests the <see cref="AddressValidator.ValidateAddress(string)"/> method to ensure that
    /// it throws a <see cref="ValidationException"/> when the address is invalid.
    /// </summary>
    [Fact]
    public void ValidateAddress_InvalidAddress_ThrowsValidationException()
    {
        // Arrange
        var invalidAddress = ""; // Empty address is invalid

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => AddressValidator.ValidateAddress(invalidAddress));
        Assert.Equal(ValidationErrorCode.InvalidAddress, exception.ErrorCode);
    }

    /// <summary>
    /// Tests the <see cref="AddressValidator.IsValidAddress(string)"/> method to ensure it returns
    /// true when the address is valid.
    /// </summary>
    [Fact]
    public void IsValidAddress_ValidAddress_ReturnsTrue()
    {
        // Arrange
        var validAddress = "456 Oak Avenue, Smalltown";

        // Act
        var result = AddressValidator.IsValidAddress(validAddress);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests the <see cref="AddressValidator.IsValidAddress(string)"/> method to ensure it returns
    /// false when the address is invalid.
    /// </summary>
    [Fact]
    public void IsValidAddress_InvalidAddress_ReturnsFalse()
    {
        // Arrange
        var invalidAddress = "   "; // Address is only whitespace, hence invalid

        // Act
        var result = AddressValidator.IsValidAddress(invalidAddress);

        // Assert
        Assert.False(result);
    }
}
}
