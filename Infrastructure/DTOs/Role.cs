using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DTOs;

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
}
