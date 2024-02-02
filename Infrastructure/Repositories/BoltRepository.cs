using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class BoltRepository(DataContext context) : BaseRepository<BoltEntity>(context)
{

    private readonly DataContext _context = context;


}

