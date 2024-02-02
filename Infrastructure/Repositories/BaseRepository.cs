using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;

        protected BaseRepository(DataContext context)
        {
            _context = context;
        }

        public virtual TEntity Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return null!;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _context.Set<TEntity>().ToList();
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return null!;

        }

        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _context.Set<TEntity>().FirstOrDefault(predicate, null!);
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return null!;
        }

        public virtual TEntity Update(TEntity entity)
        {
            try
            {
                var entityToUpdate = _context.Set<TEntity>().Find(entity);
                if (entityToUpdate != null)
                {
                    entityToUpdate = entity;
                    _context.Set<TEntity>().Update(entityToUpdate);
                    _context.SaveChanges();

                    return entityToUpdate;
                }
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return null!;
        }

        public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entityToDelete = _context.Set<TEntity>().FirstOrDefault(predicate);
                if (entityToDelete != null)
                {
                    _context.Set<TEntity>().Remove(entityToDelete);
                    _context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return true;
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _context.Set<TEntity>().Any(predicate);
            }
            catch (Exception ex) { Debug.WriteLine("Error :: " + ex.Message); }
            return false;
        }

    }

}



