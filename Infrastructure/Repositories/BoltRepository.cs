using Infrastructure.Contexts;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class BoltRepository(DataContext context) : BaseRepository<BoltEntity>(context)
{

    private readonly DataContext _context = context;

    public BoltEntity SelectBoltName(string boltName)
    {
        try
        {
            return _context.Set<BoltEntity>().FirstOrDefault(x => x.BoltName == boltName)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
            throw; // Rethrow the exception to handle it at a higher level
        }
    }

    public BoltEntity SelectBoltSize(decimal boltSize)
    {
        try
        {
            return _context.Set<BoltEntity>().FirstOrDefault(x => x.BoltSize == boltSize)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
            throw; // Rethrow the exception to handle it at a higher level
        }
    }

    public BoltEntity SelectRoleId(int id)
    {

        try
        {
            return _context.Set<BoltEntity>().Find(id)!;
        }
        catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
        return null!;
    }

}

