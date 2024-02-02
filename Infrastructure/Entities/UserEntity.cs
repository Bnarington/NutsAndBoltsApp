
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infrastructure.Entities;

public class UserEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;
    [Required, ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    public virtual RoleEntity Role { get; set; } = null!;


}
