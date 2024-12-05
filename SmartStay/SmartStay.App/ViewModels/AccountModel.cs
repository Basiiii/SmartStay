using System.ComponentModel.DataAnnotations;

namespace SmartStay.App.ViewModels
{
/// <summary>
/// Represents a user account model for the SmartStay application, ensuring validation using predefined validators and
/// error codes.
/// </summary>
public class AccountViewModel
{
    // First Name
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    // Last Name
    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    // Email
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    // Phone Number
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string PhoneNumber { get; set; }

    // Address
    [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; }

    // Is Host
    public bool IsHost { get; set; }
}
}
