/// <copyright file="AccommodationType.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="AccommodationType"/> enumeration used in the SmartStay
/// application, representing different accommodation types available for booking.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>07/10/2024</date>

/// <summary>
/// This namespace contains enumerations used within the SmartStay application.
/// </summary>
namespace SmartStay.Common.Enums
{
/// <summary>
/// Enumeration representing different types of accommodations available for booking.
/// </summary>
public enum AccommodationType
{
    /// <summary>
    /// Indicates that the accommodation type is not defined. This is used when the accommodation type is not chosen.
    /// </summary>
    None,

    /// <summary>
    /// Represents a traditional hotel accommodation, typically offering private rooms and common amenities.
    /// </summary>
    Hotel,

    /// <summary>
    /// Represents a standalone house accommodation, ideal for private stays and larger groups.
    /// </summary>
    House,

    /// <summary>
    /// Represents an apartment accommodation, typically part of a larger building, offering self-contained living
    /// space.
    /// </summary>
    Apartment,

    /// <summary>
    /// Represents a villa accommodation, usually a larger, luxury residence often with a private pool and garden.
    /// </summary>
    Villa,

    /// <summary>
    /// Represents a bed and breakfast accommodation, providing a private room with breakfast included, often in a home
    /// setting.
    /// </summary>
    BedAndBreakfast,

    /// <summary>
    /// Represents a hostel accommodation, often offering dormitory-style rooms and shared facilities, popular among
    /// budget travelers.
    /// </summary>
    Hostel,

    /// <summary>
    /// Represents a resort accommodation, typically offering all-inclusive services and multiple leisure amenities
    /// on-site.
    /// </summary>
    Resort,

    /// <summary>
    /// Represents a cottage accommodation, usually a small, cozy house in a rural or nature setting.
    /// </summary>
    Cottage,

    /// <summary>
    /// Represents a cabin accommodation, typically a small, rustic structure often located in remote or forested areas.
    /// </summary>
    Cabin,

    /// <summary>
    /// Represents a guesthouse accommodation, which offers a private room within a larger property, usually with shared
    /// amenities.
    /// </summary>
    Guesthouse,

    /// <summary>
    /// Represents a chalet accommodation, usually a wooden house located in mountain regions, popular for ski
    /// vacations.
    /// </summary>
    Chalet,

    /// <summary>
    /// Represents a lodge accommodation, typically found in nature destinations, offering basic to luxurious amenities.
    /// </summary>
    Lodge
}
}
