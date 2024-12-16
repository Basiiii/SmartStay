/// <copyright file="OwnerController.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="OwnerController"/> class, which provides API endpoints for managing
/// owner-related operations. It facilitates creating, retrieving, updating, and removing owners using
/// the OwnerManager service for core logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>13/12/2024</date>
using Microsoft.AspNetCore.Mvc;
using SmartStay.Core.Services;
using SmartStay.Core.Models;
using SmartStay.Common.Exceptions;
using SmartStay.Validation;
using SmartStay.Common.Enums;
using SmartStay.Api.DTOs.Owner;

/// <summary>
/// The <c>SmartStay.API.Controllers</c> namespace contains the controllers used to define API endpoints
/// for the application. These controllers handle HTTP requests and return appropriate responses.
/// </summary>
namespace SmartStay.API.Controllers
{
/// <summary>
/// Handles operations related to owners.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OwnerController : ControllerBase
{
    private readonly BookingManager _bookingManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="OwnerController"/> class.
    /// </summary>
    /// <param name="bookingManager">The booking manager service.</param>
    public OwnerController(BookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    /// <summary>
    /// Creates a new owner with basic information.
    /// </summary>
    /// <param name="request">The owner creation request containing basic details.</param>
    /// <returns>The newly created owner.</returns>
    /// <response code="201">Returns the newly created owner.</response>
    /// <response code="400">If the owner creation fails.</response>
    [HttpPost("basic")]
    [ProducesResponseType(typeof(Owner), 201)]
    [ProducesResponseType(400)]
    public ActionResult<Owner> CreateBasicOwner([FromBody] CreateBasicOwnerRequest request)
    {
        try
        {
            Owner newOwner = _bookingManager.CreateBasicOwner(request.FirstName, request.LastName, request.Email);
            return CreatedAtAction(nameof(GetOwner), new { id = newOwner.Id }, newOwner);
        }
        catch (OwnerCreationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new owner with complete information.
    /// </summary>
    /// <param name="request">The owner creation request containing all details.</param>
    /// <returns>The newly created owner.</returns>
    /// <response code="201">Returns the newly created owner.</response>
    /// <response code="400">If the owner creation fails.</response>
    [HttpPost("complete")]
    [ProducesResponseType(typeof(Owner), 201)]
    [ProducesResponseType(400)]
    public ActionResult<Owner> CreateCompleteOwner([FromBody] CreateCompleteOwnerRequest request)
    {
        try
        {
            Owner newOwner = _bookingManager.CreateCompleteOwner(request.FirstName, request.LastName, request.Email,
                                                                 request.PhoneNumber, request.Address);
            return CreatedAtAction(nameof(GetOwner), new { id = newOwner.Id }, newOwner);
        }
        catch (OwnerCreationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves an owner by their ID.
    /// </summary>
    /// <param name="id">The ID of the owner to retrieve.</param>
    /// <returns>The owner with the specified ID.</returns>
    /// <response code="200">Returns the owner with the specified ID.</response>
    /// <response code="404">If the owner is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Owner), 200)]
    [ProducesResponseType(404)]
    public ActionResult<Owner> GetOwner(int id)
    {
        try
        {
            Owner owner = _bookingManager.FindOwnerById(id);
            return Ok(owner);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates the details of an existing owner.
    /// </summary>
    /// <param name="id">The unique ID of the owner to update.</param>
    /// <param name="updateRequest">The owner update request containing the new details.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the owner was updated successfully.</response>
    /// <response code="404">If the owner is not found.</response>
    /// <response code="400">If any validation errors occur during the update process.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public ActionResult UpdateOwner(int id, [FromBody] UpdateOwnerRequest updateRequest)
    {
        var result = _bookingManager.UpdateOwner(id, updateRequest.FirstName, updateRequest.LastName,
                                                 updateRequest.Email, updateRequest.PhoneNumber, updateRequest.Address);

        return result switch {
            UpdateOwnerResult.Success => Ok(new { message = "Owner updated successfully." }),
            UpdateOwnerResult.OwnerNotFound => NotFound(new { message = $"Owner with ID {id} not found." }),
            UpdateOwnerResult.InvalidFirstName => BadRequest(new { message = "Invalid first name." }),
            UpdateOwnerResult.InvalidLastName => BadRequest(new { message = "Invalid last name." }),
            UpdateOwnerResult.InvalidEmail => BadRequest(new { message = "Invalid email address." }),
            UpdateOwnerResult.InvalidPhoneNumber => BadRequest(new { message = "Invalid phone number." }),
            UpdateOwnerResult.InvalidAddress => BadRequest(new { message = "Invalid address." }),
            _ => StatusCode(500, new { message = "An unexpected error occurred." })
        };
    }

    /// <summary>
    /// Removes an owner from the system.
    /// </summary>
    /// <param name="id">The unique ID of the owner to remove.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the owner was successfully removed.</response>
    /// <response code="404">If the owner with the specified ID was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    public ActionResult RemoveOwner(int id)
    {
        var wasRemoved = _bookingManager.RemoveOwner(id);

        if (wasRemoved)
        {
            return Ok(new { message = $"Owner with ID {id} was successfully removed." });
        }
        else
        {
            return NotFound(new { message = $"Owner with ID {id} was not found." });
        }
    }
}
}
