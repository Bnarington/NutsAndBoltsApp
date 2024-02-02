using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context)
{
    private readonly DataContext _context = context;

    public override IEnumerable<UserEntity> GetAll()
    {
        try
        {
            return _context.Users.Include(x => x.Role).ToList();


        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            return _context.Users.Include(x => x.Role).FirstOrDefault(predicate, null!);
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }
}
