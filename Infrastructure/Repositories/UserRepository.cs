using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public override IEnumerable<UserEntity> GetAll()
        {
            try
            {
                // Include related entities using EF Core Include method
                return _context.Users.Include(x => x.Role).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                throw; // Rethrow the exception to propagate it
            }
        }

        public override UserEntity GetOne(Expression<Func<UserEntity, bool>> predicate)
        {
            try
            {
                // Simplify the query and avoid using null!
                return _context.Users.Include(x => x.Role).FirstOrDefault(predicate)!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                throw; // Rethrow the exception to propagate it
            }
        }
    }
}
