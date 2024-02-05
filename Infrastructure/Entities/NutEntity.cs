using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(NutName))]
public class NutEntity
{
    [Key]
    public int Id { get; set; }
    public string NutName { get; set; } = null!;
    [Precision(18, 2)]
    public decimal? NutSize { get; set; }
    
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
