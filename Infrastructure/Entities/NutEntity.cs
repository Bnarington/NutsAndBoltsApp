using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(NutName))]
public class NutEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string NutName { get; set; } = null!;
    [Required]
    public string NutSize { get; set; } = null!;
    
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
