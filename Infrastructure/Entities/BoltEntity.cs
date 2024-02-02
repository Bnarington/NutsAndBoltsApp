using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(BoltName))]
public class BoltEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string BoltName { get; set; } = null!;

    [Required]
    public string BoltSize { get; set; } = null!;

    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
