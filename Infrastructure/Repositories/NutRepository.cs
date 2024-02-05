using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class NutRepository(DataContext context) : BaseRepository<NutEntity>(context)
{
    private readonly DataContext _context = context;

}
