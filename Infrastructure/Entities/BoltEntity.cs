using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

[Index(nameof(BoltName))]
public class BoltEntity
{

    [Key]
    public string BoltName { get; set; } = null!;

    public decimal? BoltSize { get; set; }

    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
