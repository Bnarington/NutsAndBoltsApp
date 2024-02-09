using Infrastructure.Contexts;
using Infrastructure.Entities;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class NutRepository(DataContext context) : BaseRepository<NutEntity>(context)
{
    private readonly DataContext _context = context;

    public NutEntity SelectNutName(string nutName)
    {
        try
        {
            return _context.Set<NutEntity>().FirstOrDefault(x => x.NutName == nutName)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
            throw; // Rethrow the exception to handle it at a higher level
        }
    }

    public NutEntity SelectNutSize(decimal nutSize)
    {
        try
        {
            return _context.Set<NutEntity>().FirstOrDefault(x => x.NutSize == nutSize)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error :: " + ex.Message);
            throw; // Rethrow the exception to handle it at a higher level
        }
    }
}
