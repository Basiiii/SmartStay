/// <copyright file="ReservationController.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="ReservationController"/> class, which provides API endpoints for managing
/// reservation-related operations. It facilitates creating, retrieving, updating, and removing reservations using
/// the ReservationManager service for core logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>13/12/2024</date>
using Microsoft.AspNetCore.Mvc;
using SmartStay.Core.Services;
using SmartStay.Core.Models;
using SmartStay.Common.Exceptions;
using SmartStay.Validation;
using SmartStay.Common.Enums;
using SmartStay.Api.DTOs.Reservation;

/// <summary>
/// The <c>SmartStay.API.Controllers</c> namespace contains the controllers used to define API endpoints
/// for the application. These controllers handle HTTP requests and return appropriate responses.
/// </summary>
namespace SmartStay.API.Controllers
{
/// <summary>
/// Handles operations related to reservations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    private readonly BookingManager _bookingManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReservationController"/> class.
    /// </summary>
    /// <param name="bookingManager">The booking manager service.</param>
    public ReservationController(BookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    /// <summary>
    /// Creates a new reservation.
    /// </summary>
    /// <param name="request">The reservation creation request containing necessary details.</param>
    /// <returns>The newly created reservation.</returns>
    /// <response code="201">Returns the newly created reservation.</response>
    /// <response code="400">If the reservation creation fails.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Reservation), 201)]
    [ProducesResponseType(400)]
    public ActionResult<Reservation> CreateReservation([FromBody] CreateReservationRequest request)
    {
        try
        {
            Reservation newReservation = _bookingManager.CreateReservation(
                request.ClientId, request.AccommodationId, request.RoomId, request.CheckIn, request.CheckOut);
            return CreatedAtAction(nameof(GetReservation), new { id = newReservation.Id }, newReservation);
        }
        catch (ReservationCreationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a reservation by its ID.
    /// </summary>
    /// <param name="id">The ID of the reservation to retrieve.</param>
    /// <returns>The reservation with the specified ID.</returns>
    /// <response code="200">Returns the reservation with the specified ID.</response>
    /// <response code="404">If the reservation is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Reservation), 200)]
    [ProducesResponseType(404)]
    public ActionResult<Reservation> GetReservation(int id)
    {
        Reservation? reservation = _bookingManager.Reservations.FindReservationById(id);
        if (reservation == null)
        {
            return NotFound(new { message = "Reservation not found." });
        }
        else
        {

            return Ok(reservation);
        }
    }

    /// <summary>
    /// Updates the details of an existing reservation.
    /// </summary>
    /// <param name="id">The unique ID of the reservation to update.</param>
    /// <param name="updateRequest">The reservation update request containing the new details.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the reservation was updated successfully.</response>
    /// <response code="404">If the reservation is not found.</response>
    /// <response code="400">If any validation errors occur during the update process.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public ActionResult UpdateReservation(int id, [FromBody] UpdateReservationRequest updateRequest)
    {
        var result = _bookingManager.UpdateReservation(id, updateRequest.NewCheckIn, updateRequest.NewCheckOut);

        return result switch { UpdateReservationResult.Success =>
                                   Ok(new { message = "Reservation updated successfully." }),

                               UpdateReservationResult.ReservationNotFound =>
                                   NotFound(new { message = $"Reservation with ID {id} not found." }),

                               UpdateReservationResult.InvalidDates =>
                                   BadRequest(new { message = "Invalid check-in or check-out date." }),

                               UpdateReservationResult.DatesUnavailable =>
                                   BadRequest(new { message = "The room is unavailable for the specified dates." }),

                               UpdateReservationResult.AccommodationNotFound =>
                                   NotFound(new { message = "Associated accommodation or room not found." }),

                               UpdateReservationResult.RoomNotFound =>
                                   NotFound(new { message = "Associated accommodation or room not found." }),

                               _ => StatusCode(500, new { message = "An unexpected error occurred." }) };
    }

    /// <summary>
    /// Removes a reservation from the system.
    /// </summary>
    /// <param name="id">The unique ID of the reservation to remove.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the reservation was successfully removed.</response>
    /// <response code="404">If the reservation with the specified ID was not found.</response>
    /// <response code="500">If an error occurred during the removal process.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public ActionResult RemoveReservation(int id)
    {
        var cancellationResult = _bookingManager.CancelReservation(id);

        switch (cancellationResult)
        {
        case CancellationResult.Success:
            return Ok(new { message = $"Reservation with ID {id} was successfully removed." });

        case CancellationResult.ReservationNotFound:
            return NotFound(new { message = $"Reservation with ID {id} was not found." });

        case CancellationResult.AccommodationNotFound:
        case CancellationResult.RoomNotFound:
            return NotFound(
                new { message = $"Reservation with ID {id} could not be removed due to missing associated entities." });

        case CancellationResult.Error:
        default:
            return StatusCode(
                500, new { message = $"An error occurred while attempting to remove reservation with ID {id}." });
        }
    }
}
}
