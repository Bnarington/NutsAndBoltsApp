using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities;

public class ProductEntity
{
    [Key]
    public string ArticleNumber { get; set; } = null!;
    [Required, StringLength(50)]
    public string Company { get; set; } = null!;
    [StringLength(50)]
    public string? Description { get; set; }
    [StringLength(50)]
    public string? Ingress { get; set; }
    [Required, Column(TypeName = "money")]
    public decimal Price { get; set; }
    [ForeignKey(nameof(BoltEntity))]
    public string BoltName { get; set; } = null!;
    public decimal? BoltSize { get; set; }
    [ForeignKey(nameof(NutEntity))]
    public string NutName { get; set; } = null!;
    public decimal? NutSize { get; set; }


    public virtual NutEntity Nut { get; set; } = null!;
    public virtual BoltEntity Bolt { get; set; } = null!;

}
