/// <copyright file="RoomType.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="RoomType"/> enumeration used in the SmartStay
/// application, representing different room types available within accommodations.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>27/11/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing different types of rooms available within accommodations.
/// </summary>
public enum RoomType
{
    /// <summary>
    /// Indicates that the room type is not defined. This is used when the room type is not chosen.
    /// </summary>
    None,

    /// <summary>
    /// Represents a single room, typically designed for one occupant with a single bed.
    /// </summary>
    Single,

    /// <summary>
    /// Represents a double room, typically designed for two occupants with a double bed or two single beds.
    /// </summary>
    Double,

    /// <summary>
    /// Represents a twin room, featuring two separate single beds for two occupants.
    /// </summary>
    Twin,

    /// <summary>
    /// Represents a suite, offering a more spacious and luxurious setup, often with separate living and sleeping areas.
    /// </summary>
    Suite,

    /// <summary>
    /// Represents a family room, designed to accommodate larger groups or families, often with multiple beds.
    /// </summary>
    Family,

    /// <summary>
    /// Represents a studio room, typically featuring an open-plan design with combined sleeping, living, and
    /// kitchenette areas.
    /// </summary>
    Studio,

    /// <summary>
    /// Represents a deluxe room, offering premium amenities and a more luxurious experience compared to standard rooms.
    /// </summary>
    Deluxe,

    /// <summary>
    /// Represents a penthouse room, usually located on the top floor with luxurious features and expansive views.
    /// </summary>
    Penthouse,

    /// <summary>
    /// Represents a dormitory-style room, typically featuring multiple beds in a shared space, common in hostels.
    /// </summary>
    Dormitory,

    /// <summary>
    /// Represents an accessible room, specifically designed for guests with disabilities, ensuring barrier-free access
    /// and amenities.
    /// </summary>
    Accessible,

    /// <summary>
    /// Represents a presidential suite, offering the highest level of luxury and space within an accommodation, often
    /// with exclusive services.
    /// </summary>
    PresidentialSuite
}
}
