/// <copyright file="ClientController.cs">
/// Copyright (c) 2024 Enrique Rodrigues. All Rights Reserved.
/// </copyright>
/// <file>
/// This file contains the <see cref="ClientController"/> class, which provides API endpoints for managing
/// client-related operations. It facilitates creating new clients and retrieving client details, leveraging the
/// BookingManager service for core logic.
/// </file>
/// <author>Enrique Rodrigues</author>
/// <date>05/12/2024</date>
using Microsoft.AspNetCore.Mvc;
using SmartStay.Core.Services;
using SmartStay.Core.Models;
using SmartStay.Common.Exceptions;
using SmartStay.Api.DTOs.Client;
using SmartStay.Validation;
using SmartStay.Common.Enums;

/// <summary>
/// The <c>SmartStay.API.Controllers</c> namespace contains the controllers used to define API endpoints
/// for the SmartStay application. These controllers act as the interface between the client and server,
/// handling HTTP requests and returning appropriate responses.
/// </summary>
namespace SmartStay.API.Controllers
{
/// <summary>
/// Handles operations related to clients.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly BookingManager _bookingManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientController"/> class.
    /// </summary>
    /// <param name="bookingManager">The booking manager service.</param>
    public ClientController(BookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    /// <summary>
    /// Creates a new client with basic information.
    /// </summary>
    /// <param name="request">The client creation request containing basic details.</param>
    /// <returns>The newly created client.</returns>
    /// <response code="201">Returns the newly created client.</response>
    /// <response code="400">If the client creation fails.</response>
    [HttpPost("basic")]
    [ProducesResponseType(typeof(Client), 201)]
    [ProducesResponseType(400)]
    public ActionResult<Client> CreateBasicClient([FromBody] CreateBasicClientRequest request)
    {
        try
        {
            // Call the CreateBasicClient method from the BookingManager service
            Client newClient = _bookingManager.CreateBasicClient(request.FirstName, request.LastName, request.Email);

            // Return the created client with a 201 status code
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }
        catch (ClientCreationException ex)
        {
            // Handle exceptions and return a bad request or appropriate status
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new client with complete information.
    /// </summary>
    /// <param name="request">The client creation request containing all details.</param>
    /// <returns>The newly created client.</returns>
    /// <response code="201">Returns the newly created client.</response>
    /// <response code="400">If the client creation fails.</response>
    [HttpPost("complete")]
    public ActionResult<Client> CreateCompleteClient([FromBody] CreateCompleteClientRequest request)
    {
        try
        {
            // Call the CreateCompleteClient method from the BookingManager service
            Client newClient = _bookingManager.CreateCompleteClient(request.FirstName, request.LastName, request.Email,
                                                                    request.PhoneNumber, request.Address);

            // Return the created client with a 201 status code
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }
        catch (ClientCreationException ex)
        {
            // Handle exceptions and return a bad request or appropriate status
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a client by their ID.
    /// </summary>
    /// <param name="id">The ID of the client to retrieve.</param>
    /// <returns>The client with the specified ID.</returns>
    /// <response code="200">Returns the client with the specified ID.</response>
    /// <response code="404">If the client is not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Client), 200)]
    [ProducesResponseType(404)]
    public ActionResult<Client> GetClient(int id)
    {
        try
        {
            // Call the FindClientById method from the BookingManager service
            Client client = _bookingManager.FindClientById(id);

            // Return the client with a 200 status code
            return Ok(client);
        }
        catch (ArgumentException ex)
        {
            // Log the error and return a 404 Not Found response
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            // Log the error and return a 400 Bad Request response
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates the details of an existing client.
    /// </summary>
    /// <param name="id">The unique ID of the client to update.</param>
    /// <param name="updateRequest">The client update request containing the new details.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the client was updated successfully.</response>
    /// <response code="404">If the client is not found.</response>
    /// <response code="400">If any validation errors occur during the update process.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult UpdateClient(int id, [FromBody] UpdateClientRequest updateRequest)
    {
        // Call the UpdateClient method from BookingManager
        var result =
            _bookingManager.UpdateClient(id, updateRequest.FirstName, updateRequest.LastName, updateRequest.Email,
                                         updateRequest.PhoneNumber, updateRequest.Address, updateRequest.PaymentMethod);

        // Handle the result of the update operation
        return result switch {
            UpdateClientResult.Success => Ok(new { message = "Client updated successfully." }),
            UpdateClientResult.ClientNotFound => NotFound(new { message = $"Client with ID {id} not found." }),
            UpdateClientResult.InvalidFirstName => BadRequest(new { message = "Invalid first name." }),
            UpdateClientResult.InvalidLastName => BadRequest(new { message = "Invalid last name." }),
            UpdateClientResult.InvalidEmail => BadRequest(new { message = "Invalid email address." }),
            UpdateClientResult.InvalidPhoneNumber => BadRequest(new { message = "Invalid phone number." }),
            UpdateClientResult.InvalidAddress => BadRequest(new { message = "Invalid address." }),
            UpdateClientResult.InvalidPaymentMethod => BadRequest(new { message = "Invalid payment method." }),
            _ => StatusCode(500, new { message = "An unexpected error occurred." })
        };
    }

    /// <summary>
    /// Removes a client from the system.
    /// </summary>
    /// <param name="id">The unique ID of the client to remove.</param>
    /// <returns>An appropriate status code indicating the result of the operation.</returns>
    /// <response code="200">If the client was successfully removed.</response>
    /// <response code="404">If the client with the specified ID was not found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] // Successful removal
    [ProducesResponseType(StatusCodes.Status404NotFound)]           // Client not found
    public ActionResult RemoveClient(int id)
    {
        var wasRemoved = _bookingManager.RemoveClient(id);

        if (wasRemoved)
        {
            return Ok(new { message = $"Client with ID {id} was successfully removed." });
        }
        else
        {
            return NotFound(new { message = $"Client with ID {id} was not found." });
        }
    }
}
}
