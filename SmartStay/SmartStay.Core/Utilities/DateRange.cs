
using ProtoBuf;

/// <copyright file="DateRange.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="DateRange"/> class, which represents a range of dates.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>21/11/2024</date>

/// <summary>
/// The <c>SmartStay.Utilities</c> namespace provides helper functions and utility classes used throughout the SmartStay
/// application. These utilities support common operations and enhance reusability across different components of the
/// application.
/// </summary>
namespace SmartStay.Core.Utilities
{
/// <summary>
/// Represents a range of dates with a start and end date.
/// Implements <see cref="IComparable{DateRange}"/> to allow sorting and comparisons.
/// </summary>
[ProtoContract]
public class DateRange : IComparable<DateRange>
{
    /// <summary>
    /// Gets or sets the start date of the range.
    /// </summary>
    [ProtoMember(1)]
    public DateTime Start { get; set; }

    /// <summary>
    /// Gets or sets the end date of the range.
    /// </summary>
    [ProtoMember(2)]
    public DateTime End {
        get; set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateRange"/> class.
    /// </summary>
    /// <param name="start">The start date of the range.</param>
    /// <param name="end">The end date of the range.</param>
    public DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Compares the current <see cref="DateRange"/> with another <see cref="DateRange"/> to determine their relative
    /// order.
    /// </summary>
    /// <param name="other">The other <see cref="DateRange"/> to compare with.</param>
    /// <returns>
    /// A value less than zero if this instance precedes <paramref name="other"/>;
    /// zero if this instance occurs at the same position in the sort order;
    /// and a value greater than zero if this instance follows <paramref name="other"/>.
    /// </returns>
    public int CompareTo(DateRange? other)
    {
        if (other is null)
            return 1; // If other is null, this object is considered greater.

        // First, compare by the Start date.
        int startComparison = Start.CompareTo(other.Start);
        if (startComparison != 0)
            return startComparison;

        // If Start dates are the same, compare by End date.
        return End.CompareTo(other.End);
    }

    /// <summary>
    /// Determines whether the current <see cref="DateRange"/> is equal to another <see cref="DateRange"/>.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if the current <see cref="DateRange"/> is equal to the other <see cref="DateRange"/>;
    /// otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is DateRange other)
        {
            // Check if both Start and End dates are equal.
            return Start == other.Start && End == other.End;
        }
        return false;
    }

    /// <summary>
    /// Returns a hash code for this <see cref="DateRange"/> based on its Start and End dates.
    /// </summary>
    /// <returns>A hash code for the current <see cref="DateRange"/>.</returns>
    public override int GetHashCode()
    {
        // Combine the hash codes of Start and End dates to get a unique hash code.
        return HashCode.Combine(Start, End);
    }

    /// <summary>
    /// Defines equality comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the <see cref="DateRange"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(DateRange left, DateRange right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Defines inequality comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the <see cref="DateRange"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(DateRange left, DateRange right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Defines the less-than comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the first <see cref="DateRange"/> precedes the second; otherwise,
    /// <c>false</c>.</returns>
    public static bool operator<(DateRange left, DateRange right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Defines the less-than-or-equal comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the first <see cref="DateRange"/> precedes or is equal to the second; otherwise,
    /// <c>false</c>.</returns>
    public static bool operator <=(DateRange left, DateRange right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Defines the greater-than comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the first <see cref="DateRange"/> follows the second; otherwise, <c>false</c>.</returns>
    public static bool operator>(DateRange left, DateRange right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Defines the greater-than-or-equal comparison for <see cref="DateRange"/> instances.
    /// </summary>
    /// <param name="left">The first <see cref="DateRange"/>.</param>
    /// <param name="right">The second <see cref="DateRange"/>.</param>
    /// <returns><c>true</c> if the first <see cref="DateRange"/> follows or is equal to the second; otherwise,
    /// <c>false</c>.</returns>
    public static bool operator >=(DateRange left, DateRange right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Creates a deep copy of the current <see cref="DateRange"/> instance.
    /// </summary>
    /// <returns>A new <see cref="DateRange"/> instance with identical data to the current instance.</returns>
    public DateRange Clone()
    {
        // Use the constructor to initialize the new DateRange
        return new DateRange(Start, End);
    }
}
}
