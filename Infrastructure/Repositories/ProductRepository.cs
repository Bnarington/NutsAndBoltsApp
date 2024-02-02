using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class ProductRepository(DataContext context) : BaseRepository<ProductEntity>(context)
{
    private readonly DataContext _context = context;

    public override IEnumerable<ProductEntity> GetAll()
    {
        try
        {
            return _context.Products.Include(x => x.Bolt).Include(x => x.Nut).ToList();


        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override ProductEntity GetOne(Expression<Func<ProductEntity, bool>> predicate)
    {
        try
        {
            return _context.Products.Include(x => x.Bolt).Include(x => x.Nut).FirstOrDefault(predicate, null!);
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

}
