/// <copyright file="RoomValidator.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the implementation of the <see cref="RoomValidator"/> class,
/// which provides methods for validating various aspects of rooms such as type,
/// occupancy, amenities, and availability within the SmartStay application.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>27/11/2024</date>
using SmartStay.Common.Enums;

/// <summary>
/// The <c>SmartStay.Validation.Validators</c> namespace provides classes and methods
/// dedicated to validating various aspects of the SmartStay application. These validations
/// ensure that input data adheres to business requirements and standards.
/// </summary>
namespace SmartStay.Validation.Validators
{
/// <summary>
/// The <c>RoomValidator</c> class provides methods for validating room-related data within
/// the SmartStay application. It ensures integrity and compliance with business rules.
/// </summary>
public static class RoomValidator
{
    /// <summary>
    /// Validates a room type, throwing an exception if invalid.
    /// </summary>
    /// <param name="roomType">The room type to validate.</param>
    /// <returns>The validated room type if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the room type is invalid.</exception>
    public static RoomType ValidateRoomType(RoomType roomType)
    {
        if (!IsValidRoomType(roomType))
        {
            throw new ValidationException(ValidationErrorCode.InvalidRoomType);
        }
        return roomType;
    }

    /// <summary>
    /// Checks if a room type is valid by confirming it is a defined enum value.
    /// </summary>
    /// <param name="roomType">The room type to check.</param>
    /// <returns><c>true</c> if the room type is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidRoomType(RoomType roomType) => Enum.IsDefined(typeof(RoomType), roomType);

    /// <summary>
    /// Validates the availability status of a room, throwing an exception if invalid.
    /// </summary>
    /// <param name="isAvailable">The availability status to validate.</param>
    /// <returns>The validated availability status if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the availability status is invalid.</exception>
    public static bool ValidateAvailability(bool isAvailable)
    {
        if (!IsValidAvailability(isAvailable))
        {
            throw new ValidationException(ValidationErrorCode.InvalidAvailabilityStatus);
        }
        return isAvailable;
    }

    /// <summary>
    /// Checks if a room's availability status is valid.
    /// </summary>
    /// <param name="isAvailable">The availability status to check.</param>
    /// <returns><c>true</c> if the availability status is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidAvailability(bool isAvailable) => isAvailable || !isAvailable;

    /// <summary>
    /// Validates a room ID, throwing an exception if invalid.
    /// </summary>
    /// <param name="id">The room ID to validate.</param>
    /// <returns>The validated room ID if valid.</returns>
    /// <exception cref="ValidationException">Thrown if the room ID is invalid.</exception>
    public static int ValidateRoomId(int id)
    {
        if (!IsValidRoomId(id))
        {
            throw new ValidationException(ValidationErrorCode.InvalidId);
        }
        return id;
    }

    /// <summary>
    /// Checks if a room ID is valid.
    /// </summary>
    /// <param name="id">The room ID to check.</param>
    /// <returns><c>true</c> if the room ID is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidRoomId(int id) => id > 0;
}
}
