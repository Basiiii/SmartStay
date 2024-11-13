/// <copyright file="ValidationTests.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains unit tests for validation functions in the SmartStay application.
/// These tests ensure that input validation is performed correctly for different entities
/// such as names, emails, phone numbers, accommodation types, payment details, and other parameters.
/// </file>
/// <summary>
/// Defines the <see cref="ValidatorTests"/> class which contains unit tests for all validation methods
/// in the Validator class. It covers positive (valid input) and negative (invalid input) test cases for
/// methods like ValidateName, ValidateEmail, ValidatePhoneNumber, and others.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>13/11/2024</date>
/// <remarks>
/// The tests are designed to verify that the validation logic behaves as expected for various types of input.
/// Each validation method is tested for both valid and invalid inputs, ensuring the appropriate
/// results are returned or exceptions are thrown as expected. The tests also verify that error codes are correctly
/// assigned.
/// </remarks>
using SmartStay.Models.Enums;
using SmartStay.Validation;

namespace SmartStay.Tests
{
/// <summary>
/// Defines the <see cref="ValidatorTests"/> class which tests the validation logic of various input parameters
/// such as names, email addresses, accommodation types, prices, payment details, and more.
/// </summary>
[TestClass]
public class ValidatorTests
{
#region Name Validation Tests

    /// <summary>
    /// The ValidateName_ValidName_ReturnsName
    /// Tests the ValidateName method with a valid name input and ensures it returns the same name.
    /// </summary>
    /// <param name="validName">The validName<see cref="string"/></param>
    [TestMethod]
    [DataRow("John Doe")]
    public void ValidateName_ValidName_ReturnsName(string validName)
    {
        var result = Validator.ValidateName(validName);
        Assert.AreEqual(validName, result);
    }

    /// <summary>
    /// The ValidateName_InvalidName_ThrowsException
    /// Tests the ValidateName method with invalid name inputs (empty or null) and ensures a ValidationException is
    /// thrown.
    /// </summary>
    /// <param name="invalidName">The invalidName<see cref="string"/></param>
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void ValidateName_InvalidName_ThrowsException(string invalidName)
    {
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidateName(invalidName));
        Assert.AreEqual(ValidationErrorCode.InvalidName, exception.ErrorCode);
    }

#endregion

#region Accommodation Name Validation Tests

    /// <summary>
    /// The ValidateAccommodationName_ValidName_ReturnsName
    /// Tests the ValidateAccommodationName method with a valid accommodation name and ensures it returns the same name.
    /// </summary>
    /// <param name="validAccommodationName">The validAccommodationName<see cref="string"/></param>
    [TestMethod]
    [DataRow("Ocean View Resort")]
    public void ValidateAccommodationName_ValidName_ReturnsName(string validAccommodationName)
    {
        var result = Validator.ValidateAccommodationName(validAccommodationName);
        Assert.AreEqual(validAccommodationName, result);
    }

    /// <summary>
    /// The ValidateAccommodationName_InvalidName_ThrowsException
    /// Tests the ValidateAccommodationName method with invalid accommodation names (empty or null) and ensures
    /// a ValidationException is thrown.
    /// </summary>
    /// <param name="invalidAccommodationName">The invalidAccommodationName<see cref="string"/></param>
    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    public void ValidateAccommodationName_InvalidName_ThrowsException(string invalidAccommodationName)
    {
        var exception = Assert.ThrowsException<ValidationException>(
            () => Validator.ValidateAccommodationName(invalidAccommodationName));
        Assert.AreEqual(ValidationErrorCode.InvalidAccommodationName, exception.ErrorCode);
    }

#endregion

#region Email Validation Tests

    /// <summary>
    /// The ValidateEmail_ValidEmail_ReturnsEmail
    /// Tests the ValidateEmail method with a valid email input and ensures it returns the same email.
    /// </summary>
    /// <param name="validEmail">The validEmail<see cref="string"/></param>
    [TestMethod]
    [DataRow("user@example.com")]
    public void ValidateEmail_ValidEmail_ReturnsEmail(string validEmail)
    {
        var result = Validator.ValidateEmail(validEmail);
        Assert.AreEqual(validEmail, result);
    }

    /// <summary>
    /// The ValidateEmail_InvalidEmail_ThrowsException
    /// Tests the ValidateEmail method with invalid email inputs (e.g., an email missing '@')
    /// and ensures a ValidationException is thrown.
    /// </summary>
    /// <param name="invalidEmail">The invalidEmail<see cref="string"/></param>
    [TestMethod]
    [DataRow("invalid-email")]
    public void ValidateEmail_InvalidEmail_ThrowsException(string invalidEmail)
    {
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidateEmail(invalidEmail));
        Assert.AreEqual(ValidationErrorCode.InvalidEmail, exception.ErrorCode);
    }

#endregion

#region Phone Number Validation Tests

    /// <summary>
    /// The ValidatePhoneNumber_ValidPhoneNumber_ReturnsPhoneNumber
    /// Tests the ValidatePhoneNumber method with a valid phone number input and ensures it returns the same phone
    /// number.
    /// </summary>
    /// <param name="validPhoneNumber">The validPhoneNumber<see cref="string"/></param>
    [TestMethod]
    [DataRow("+351234567890")]
    public void ValidatePhoneNumber_ValidPhoneNumber_ReturnsPhoneNumber(string validPhoneNumber)
    {
        var result = Validator.ValidatePhoneNumber(validPhoneNumber);
        Assert.AreEqual(validPhoneNumber, result);
    }

    /// <summary>
    /// The ValidatePhoneNumber_InvalidPhoneNumber_ThrowsException
    /// Tests the ValidatePhoneNumber method with an invalid phone number input and ensures a ValidationException is
    /// thrown.
    /// </summary>
    /// <param name="invalidPhoneNumber">The invalidPhoneNumber<see cref="string"/></param>
    [TestMethod]
    [DataRow("12345")]
    public void ValidatePhoneNumber_InvalidPhoneNumber_ThrowsException(string invalidPhoneNumber)
    {
        var exception =
            Assert.ThrowsException<ValidationException>(() => Validator.ValidatePhoneNumber(invalidPhoneNumber));
        Assert.AreEqual(ValidationErrorCode.InvalidPhoneNumber, exception.ErrorCode);
    }

#endregion

#region Address Validation Tests

    /// <summary>
    /// The ValidateAddress_ValidAddress_ReturnsAddress
    /// Tests the ValidateAddress method with a valid address and ensures it returns the same address.
    /// </summary>
    /// <param name="validAddress">The validAddress<see cref="string"/></param>
    [TestMethod]
    [DataRow("123 Main St")]
    public void ValidateAddress_ValidAddress_ReturnsAddress(string validAddress)
    {
        var result = Validator.ValidateAddress(validAddress);
        Assert.AreEqual(validAddress, result);
    }

    /// <summary>
    /// The ValidateAddress_InvalidAddress_ThrowsException
    /// Tests the ValidateAddress method with an invalid address (e.g., empty) and ensures a ValidationException is
    /// thrown.
    /// </summary>
    /// <param name="invalidAddress">The invalidAddress<see cref="string"/></param>
    [TestMethod]
    [DataRow("")]
    public void ValidateAddress_InvalidAddress_ThrowsException(string invalidAddress)
    {
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidateAddress(invalidAddress));
        Assert.AreEqual(ValidationErrorCode.InvalidAddress, exception.ErrorCode);
    }

#endregion

#region Price Validation Tests

    /// <summary>
    /// The ValidatePrice_ValidPrice_ReturnsPrice
    /// Tests the ValidatePrice method with a valid price input and ensures it returns the same price.
    /// </summary>
    [TestMethod]
    public void ValidatePrice_ValidPrice_ReturnsPrice()
    {
        decimal validPrice = 100.0m;
        var result = Validator.ValidatePrice(validPrice);
        Assert.AreEqual(validPrice, result);
    }

    /// <summary>
    /// The ValidatePrice_InvalidPrice_ThrowsException
    /// Tests the ValidatePrice method with an invalid price input (e.g., zero or negative) and ensures
    /// a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void ValidatePrice_InvalidPrice_ThrowsException()
    {
        decimal invalidPrice = 0.0m;
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidatePrice(invalidPrice));
        Assert.AreEqual(ValidationErrorCode.InvalidPrice, exception.ErrorCode);
    }

#endregion

#region Payment Amounts Validation Tests

    /// <summary>
    /// The ValidatePaymentAmount_ValidAmount_ReturnsAmount
    /// Tests the ValidatePaymentAmount method with a valid payment amount input and ensures it returns the same amount.
    /// </summary>
    [TestMethod]
    public void ValidatePaymentAmount_ValidAmount_ReturnsAmount()
    {
        decimal validAmount = 150.0m;
        var result = Validator.ValidatePaymentAmount(validAmount);
        Assert.AreEqual(validAmount, result);
    }

    /// <summary>
    /// The ValidatePaymentAmount_InvalidAmount_ThrowsException
    /// Tests the ValidatePaymentAmount method with an invalid payment amount input (e.g., negative amount)
    /// and ensures a ValidationException is thrown.
    /// </summary>
    [TestMethod]
    public void ValidatePaymentAmount_InvalidAmount_ThrowsException()
    {
        decimal invalidAmount = -10.0m;
        var exception =
            Assert.ThrowsException<ValidationException>(() => Validator.ValidatePaymentAmount(invalidAmount));
        Assert.AreEqual(ValidationErrorCode.InvalidPaymentValue, exception.ErrorCode);
    }

#endregion

#region Payment Status Validation Tests

    /// <summary>
    /// Validates that the payment status is correctly processed when a valid status is provided.
    /// </summary>
    [TestMethod]
    public void ValidatePaymentStatus_ValidStatus_ReturnsStatus()
    {
        var validStatus = PaymentStatus.Completed;
        var result = Validator.ValidatePaymentStatus(validStatus);
        Assert.AreEqual(validStatus, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when an invalid payment status (out of the predefined enum) is provided.
    /// </summary>
    [TestMethod]
    public void ValidatePaymentStatus_InvalidStatus_ThrowsException()
    {
        var invalidStatus = (PaymentStatus)999;
        var exception =
            Assert.ThrowsException<ValidationException>(() => Validator.ValidatePaymentStatus(invalidStatus));
        Assert.AreEqual(ValidationErrorCode.InvalidPaymentStatus, exception.ErrorCode);
    }

#endregion

#region Accommodation Type Validation Tests

    /// <summary>
    /// Validates that the accommodation type is correctly processed when a valid type is provided.
    /// </summary>
    [TestMethod]
    public void ValidateAccommodationType_ValidType_ReturnsType()
    {
        var validType = AccommodationType.Hotel;
        var result = Validator.ValidateAccommodationType(validType);
        Assert.AreEqual(validType, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when an invalid accommodation type (out of the predefined enum) is
    /// provided.
    /// </summary>
    [TestMethod]
    public void ValidateAccommodationType_InvalidType_ThrowsException()
    {
        var invalidType = (AccommodationType)999;
        var exception =
            Assert.ThrowsException<ValidationException>(() => Validator.ValidateAccommodationType(invalidType));
        Assert.AreEqual(ValidationErrorCode.InvalidAccommodationType, exception.ErrorCode);
    }

#endregion

#region Check In Date Validation Tests

    /// <summary>
    /// Validates that the check-in date is processed correctly when a future date is provided.
    /// </summary>
    [TestMethod]
    public void ValidateCheckInDate_ValidFutureDate_ReturnsDate()
    {
        var futureDate = DateTime.Now.AddDays(1);
        var result = Validator.ValidateCheckInDate(futureDate);
        Assert.AreEqual(futureDate, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when a past date is provided as the check-in date.
    /// </summary>
    [TestMethod]
    public void ValidateCheckInDate_PastDate_ThrowsException()
    {
        var pastDate = DateTime.Now.AddDays(-1);
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidateCheckInDate(pastDate));
        Assert.AreEqual(ValidationErrorCode.InvalidDate, exception.ErrorCode);
    }

#endregion

#region Check Out Date Validation Tests

    /// <summary>
    /// Validates that the check-out date is processed correctly when it is after the check-in date.
    /// </summary>
    [TestMethod]
    public void ValidateCheckOutDate_ValidDateRange_ReturnsCheckOutDate()
    {
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = checkInDate.AddDays(1);
        var result = Validator.ValidateCheckOutDate(checkOutDate, checkInDate);
        Assert.AreEqual(checkOutDate, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when the check-out date is before the check-in date.
    /// </summary>
    [TestMethod]
    public void ValidateCheckOutDate_InvalidDateRange_ThrowsException()
    {
        var checkInDate = DateTime.Now.AddDays(1);
        var checkOutDate = checkInDate.AddDays(-1);
        var exception = Assert.ThrowsException<ValidationException>(
            () => Validator.ValidateCheckOutDate(checkOutDate, checkInDate));
        Assert.AreEqual(ValidationErrorCode.InvalidDateRange, exception.ErrorCode);
    }

#endregion

#region Total Cost Validation Tests

    /// <summary>
    /// Validates that the total cost is correctly processed when a valid cost is provided.
    /// </summary>
    [TestMethod]
    public void ValidateTotalCost_ValidCost_ReturnsTotalCost()
    {
        decimal validCost = 300.0m;
        var result = Validator.ValidateTotalCost(validCost);
        Assert.AreEqual(validCost, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when a negative cost is provided.
    /// </summary>
    [TestMethod]
    public void ValidateTotalCost_InvalidCost_ThrowsException()
    {
        decimal invalidCost = -5.0m;
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidateTotalCost(invalidCost));
        Assert.AreEqual(ValidationErrorCode.InvalidTotalCost, exception.ErrorCode);
    }

#endregion

#region Payment Validation Tests

    /// <summary>
    /// Validates that the payment amount is correctly processed when a valid payment is provided.
    /// </summary>
    [TestMethod]
    public void ValidatePayment_ValidPayment_ReturnsPayment()
    {
        decimal validPayment = 10.0m;
        var result = Validator.ValidatePayment(validPayment);
        Assert.AreEqual(validPayment, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when a negative payment value is provided.
    /// </summary>
    [TestMethod]
    public void ValidatePayment_InvalidPayment_ThrowsException()
    {
        decimal invalidPayment = -100.0m;
        var exception = Assert.ThrowsException<ValidationException>(() => Validator.ValidatePayment(invalidPayment));
        Assert.AreEqual(ValidationErrorCode.InvalidPaymentValue, exception.ErrorCode);
    }

#endregion

#region Reservation Status Validation Tests

    /// <summary>
    /// Validates that the reservation status is correctly processed when a valid status is provided.
    /// </summary>
    [TestMethod]
    public void ValidateReservationStatus_ValidStatus_ReturnsStatus()
    {
        var validStatus = ReservationStatus.Confirmed;
        var result = Validator.ValidateReservationStatus(validStatus);
        Assert.AreEqual(validStatus, result);
    }

    /// <summary>
    /// Validates that an exception is thrown when an invalid reservation status (out of the predefined enum) is
    /// provided.
    /// </summary>
    [TestMethod]
    public void ValidateReservationStatus_InvalidStatus_ThrowsException()
    {
        var invalidStatus = (ReservationStatus)999;
        var exception =
            Assert.ThrowsException<ValidationException>(() => Validator.ValidateReservationStatus(invalidStatus));
        Assert.AreEqual(ValidationErrorCode.InvalidReservationStatus, exception.ErrorCode);
    }

#endregion
}
}
