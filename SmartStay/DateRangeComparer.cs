/// <copyright file="DateRangeComparer.cs">
/// Copyright (c) 2024 All Rights Reserved
/// </copyright>
/// <file>
/// This file contains the definition of the <see cref="DateRangeComparer"/> class.
/// </file>
/// <summary>
/// Defines the <see cref="DateRangeComparer"/> class, which implements the <see cref="IComparer{T}"/> interface
/// to provide custom comparison logic for date ranges (represented as tuples of <see cref="DateTime"/> values).
/// The comparison is based on the start date of each range, used primarily for sorting or ordering date ranges.
/// </summary>
/// <author>Enrique Rodrigues</author>
/// <date>10/11/2024</date>
/// <remarks>
/// This comparer is intended to be used for sorting or performing operations on lists of date ranges,
/// where the sorting order is determined by the start date of each range.
/// </remarks>

namespace SmartStay
{
using System;
using System.Collections.Generic;

/// <summary>
/// Implements <see cref="IComparer{T}"/> to provide comparison logic for tuples of <see cref="DateTime"/> values
/// representing date ranges. The comparison is done based on the <see cref="Start"/> date of each range.
/// </summary>
public class DateRangeComparer : IComparer<(DateTime Start, DateTime End)>
{
    /// <summary>
    /// Compares two date ranges based on their start date.
    /// </summary>
    /// <param name="x">The first <see cref="DateTime"/> tuple representing a date range with a <see cref="Start"/> and
    /// <see cref="End"/> date.</param> <param name="y">The second <see cref="DateTime"/> tuple representing a date
    /// range with a <see cref="Start"/> and <see cref="End"/> date.</param> <returns> A value indicating the relative
    /// order of the two date ranges:
    /// - A negative value if <paramref name="x"/> starts before <paramref name="y"/>.
    /// - Zero if both date ranges start at the same time.
    /// - A positive value if <paramref name="x"/> starts after <paramref name="y"/>.
    /// </returns>
    public int Compare((DateTime Start, DateTime End)x, (DateTime Start, DateTime End)y)
    {
        return x.Start.CompareTo(y.Start);
    }
}
}
