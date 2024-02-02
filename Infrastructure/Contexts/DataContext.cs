
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public partial class DataContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<ProductEntity> Products { get; set; }
    public virtual DbSet<NutEntity> Nuts { get; set; }
    public virtual DbSet<BoltEntity> Bolts { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<RoleEntity> Roles { get; set; }
}
