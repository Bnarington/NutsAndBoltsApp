using Infrastructure.Contexts;
using Infrastructure.Entities;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class RoleRepository(DataContext context) : BaseRepository<RoleEntity>(context)
{
    private readonly DataContext _context = context;

    public RoleEntity SelectRoleId(int id)
    {

        try
        {
            return _context.Set<RoleEntity>().Find(id)!;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

    //public IEnumerable<RoleEntity> SelectRoleName(string roleName)
    //{
    //    try
    //    {
    //        return from x in _context.Set<RoleEntity>() where x.RoleName == roleName select x;
    //    }
    //    catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
    //    return null!;
    //}

    public RoleEntity SelectRoleName(string roleName)
    {
        try
        {
            return _context.Set<RoleEntity>().FirstOrDefault(x => x.RoleName == roleName)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
            throw; // Rethrow the exception to handle it at a higher level
        }
    }
}
