using Microsoft.AspNetCore.Mvc;
using SmartStay.Core.Services;
using SmartStay.Core.Models;
using System;
using SmartStay.Common.Exceptions;

namespace SmartStay.API.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly BookingManager _bookingManager;

    // Inject the BookingManager via the constructor
    public ClientController(BookingManager bookingManager)
    {
        _bookingManager = bookingManager;
    }

    // POST api/client
    [HttpPost]
    public ActionResult<Client> CreateClient([FromBody] CreateClientRequest request)
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

    // GET api/client/{id}
    [HttpGet("{id}")]
    public ActionResult<Client> GetClient(int id)
    {
        // You can add logic here to get a client by ID (not implemented here)
        return Ok();
    }
}

// A simple DTO (Data Transfer Object) to pass client data in the request body
public class CreateClientRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
}
