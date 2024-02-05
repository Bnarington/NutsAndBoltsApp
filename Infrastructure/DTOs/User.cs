using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class User
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}
