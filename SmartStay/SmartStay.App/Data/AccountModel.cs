using System.ComponentModel.DataAnnotations;

namespace SmartStay.App.Data
{
public class AccountModel
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }

    [Required, StringLength(50)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; }

    public string Phone { get; set; }
    public string Address { get; set; }
    public bool IsHost { get; set; }
}

}
