﻿/// <copyright file="ValidationErrorCodes.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="ValidationErrorCode"/> enum, which represents
/// specific error codes related to validation failures within the SmartStay application.
/// </file>
/// <summary>
/// Contains the <see cref="ValidationErrorCode"/> enum, which defines error codes for validation
/// errors that can occur when processing client data, such as invalid names, emails, phone numbers, addresses,
/// and payment methods.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>09/11/2024</date>
/// <remarks>
/// The <see cref="ValidationErrorCode"/> enum provides consistent error codes for validation failures
/// in the SmartStay application, enabling precise identification of validation issues and improved error handling.
/// Each enum value is associated with a unique code, which simplifies debugging and error logging.
/// </remarks>

namespace SmartStay
{
/// <summary>
/// Defines error codes for validation failures within the SmartStay application.
/// </summary>
public enum ValidationErrorCode
{
    /// <summary>
    /// Error code indicating that the provided name is invalid.
    /// </summary>
    InvalidName = 1001,

    /// <summary>
    /// Error code indicating that the provided email address is invalid.
    /// </summary>
    InvalidEmail = 1002,

    /// <summary>
    /// Error code indicating that the provided phone number is invalid.
    /// </summary>
    InvalidPhoneNumber = 1003,

    /// <summary>
    /// Error code indicating that the provided address is invalid.
    /// </summary>
    InvalidAddress = 1004,

    /// <summary>
    /// Error code indicating that the provided payment method is invalid.
    /// </summary>
    InvalidPaymentMethod = 1005,

    /// <summary>
    /// Error code indicating that the provided accommodation type is invalid.
    /// </summary>
    InvalidAccommodationType = 1006,

    /// <summary>
    /// Error code indicating that the provided ID is invalid.
    /// </summary>
    InvalidId = 1007,

    /// <summary>
    /// Error code indicating that the provided date range is invalid, typically when the check-in date is later than or
    /// equal to the check-out date.
    /// </summary>
    InvalidDateRange = 1008,

    /// <summary>
    /// Error code indicating that the provided date is invalid, typically when the date is in the past or does not meet
    /// the expected criteria.
    /// </summary>
    InvalidDate = 1009,

    /// <summary>
    /// Error code indicating that the total cost provided is invalid, usually if it is a negative value.
    /// </summary>
    InvalidTotalCost = 1010,

    /// <summary>
    /// Error code indicating that the provided payment value is invalid, such as when it is negative or exceeds the
    /// total cost.
    /// </summary>
    InvalidPaymentValue = 1011,

    /// <summary>
    /// Error code indicating that the provided reservation status is invalid, typically if it does not match any
    /// defined status in the ReservationStatus enumeration.
    /// </summary>
    InvalidReservationStatus = 1012,
}
}