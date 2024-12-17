/// <copyright file="AccommodationController.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="AccommodationController"/> class, which provides API endpoints for managing
/// accommodation-related operations. It facilitates creating, updating, and removing accommodations, leveraging
/// the AccommodationManager service for core logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>05/12/2024</date>
using Microsoft.AspNetCore.Mvc;
using SmartStay.Core.Services;
using SmartStay.Core.Models;
using SmartStay.Common.Exceptions;
using SmartStay.Common.Enums;
using SmartStay.API.DTOs.Accommodation;

/// <summary>
/// The <c>SmartStay.API.Controllers</c> namespace contains the controllers used to define API endpoints
/// for the SmartStay application. These controllers act as the interface between the client and server,
/// handling HTTP requests and returning appropriate responses.
/// </summary>
namespace SmartStay.API.Controllers
{
/// <summary>
/// Handles operations related to accommodations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AccommodationController : ControllerBase
{
    private readonly BookingManager _bookingManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccommodationController"/> class.
    /// </summary>
    /// <param name="bookingManager">The booking manager service.</param>
    public AccommodationController(BookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    /// <summary>
    /// Creates a new accommodation.
    /// </summary>
    /// <param name="request">The accommodation creation request.</param>
    /// <returns>The newly created accommodation.</returns>
    /// <response code="201">Returns the newly created accommodation.</response>
    /// <response code="400">If creation fails due to validation errors.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Accommodation), 201)]
    [ProducesResponseType(400)]
    public ActionResult<Accommodation> CreateAccommodation([FromBody] CreateAccommodationRequest request)
    {
        try
        {
            var newAccommodation =
                _bookingManager.CreateAccommodation(request.OwnerId, request.Type, request.Name, request.Address);

            return CreatedAtAction(nameof(GetAccommodation), new { id = newAccommodation.Id }, newAccommodation);
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (AccommodationCreationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates the details of an existing accommodation.
    /// </summary>
    /// <param name="id">The ID of the accommodation to update.</param>
    /// <param name="request">The update request with new details.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the accommodation was updated successfully.</response>
    /// <response code="404">If the accommodation is not found.</response>
    /// <response code="400">If validation errors occur during the update.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public ActionResult UpdateAccommodation(int id, [FromBody] UpdateAccommodationRequest request)
    {
        var result = _bookingManager.UpdateAccommodation(id, request.Type, request.Name, request.Address);

        return result switch {
            UpdateAccommodationResult.Success => Ok(new { message = "Accommodation updated successfully." }),
            UpdateAccommodationResult.AccommodationNotFound => NotFound(new { message = "Accommodation not found." }),
            UpdateAccommodationResult.InvalidType => BadRequest(new { message = "Invalid accommodation type." }),
            UpdateAccommodationResult.InvalidName => BadRequest(new { message = "Invalid name." }),
            UpdateAccommodationResult.InvalidAddress => BadRequest(new { message = "Invalid address." }),
            _ => StatusCode(500, new { message = "An unexpected error occurred." })
        };
    }

    /// <summary>
    /// Retrieves accommodation details by ID.
    /// </summary>
    /// <param name="id">The ID of the accommodation to retrieve.</param>
    /// <returns>The accommodation with the specified ID.</returns>
    /// <response code="200">Returns the accommodation details.</response>
    /// <response code="404">If the accommodation is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Accommodation), 200)]
    [ProducesResponseType(404)]
    public ActionResult<Accommodation> GetAccommodation(int id)
    {
        var accommodation = _bookingManager.Accommodations.FindAccommodationById(id);
        if (accommodation == null)
        {
            return NotFound(new { message = "Accommodation not found." });
        }
        return Ok(accommodation);
    }

    /// <summary>
    /// Removes an accommodation.
    /// </summary>
    /// <param name="id">The unique ID of the accommodation to remove.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the accommodation was successfully removed.</response>
    /// <response code="404">If the accommodation was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    public ActionResult RemoveAccommodation(int id)
    {
        var result = _bookingManager.RemoveAccommodation(id);

        return result switch {
            RemoveAccommodationResult.Success => Ok(new { message = "Accommodation removed successfully." }),
            RemoveAccommodationResult.AccommodationNotFound => NotFound(new { message = "Accommodation not found." }),
            RemoveAccommodationResult.OwnerNotFound => NotFound(new { message = "Owner not found." }),
            _ => StatusCode(500, new { message = "An unexpected error occurred." })
        };
    }
}
}
