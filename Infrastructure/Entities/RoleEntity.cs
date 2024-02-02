using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(RoleName))]
public class RoleEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
