/// <copyright file="ValidationErrorMessages.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the <see cref="ValidationErrorMessages"/> class, which is responsible for fetching localized
/// validation error messages based on the given error code. The messages are retrieved from resource files (.resx),
/// with support for multiple languages (e.g., English, French, Spanish).
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>19/11/2024</date>
using System.Globalization;
using System.Resources;

/// <summary>
/// The <c>SmartStay.Validation</c> namespace contains classes and methods for validating data and enforcing business
/// rules within the SmartStay application. These validations help ensure data integrity and compliance with application
/// requirements.
/// </summary>
namespace SmartStay.Validation
{
/// <summary>
/// Provides a mechanism to retrieve localized validation error messages based on the given <see
/// cref="ValidationErrorCode"/>. Messages are retrieved from resource files depending on the current culture of the
/// application.
/// </summary>
public static class ValidationErrorMessages
{
    /// <summary>
    /// The resource manager used to fetch localized error messages from resource files.
    /// </summary>
    private static readonly ResourceManager _resourceManager = new ResourceManager(
        "SmartStay.Validation.Resources.ValidationMessages", typeof(ValidationErrorMessages).Assembly);

    /// <summary>
    /// Retrieves the localized error message corresponding to the provided error code.
    /// The message is fetched based on the current culture (e.g., English, French, Spanish).
    /// </summary>
    /// <param name="errorCode">The error code from the <see cref="ValidationErrorCode"/> enum.</param>
    /// <returns>A localized error message string, or a fallback message if no match is found.</returns>
    public static string GetErrorMessage(ValidationErrorCode errorCode)
    {
        // Get the string based on the current culture
        return _resourceManager.GetString(errorCode.ToString(), CultureInfo.CurrentCulture) ??
               "Unknown validation error.";
    }
}
}
