using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Base repository providing basic CRUD operations for entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity managed by the repository.</typeparam>
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The data context used by the repository.
        /// </summary>
        protected readonly DataContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class with the specified data context.
        /// </summary>
        /// <param name="context">The data context.</param>
        protected BaseRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity if successful; otherwise, null.</returns>
        public virtual TEntity Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                return null!;
            }
        }

        /// <summary>
        /// Retrieves all entities of the specified type from the database.
        /// </summary>
        /// <returns>An enumerable collection of entities.</returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _context.Set<TEntity>().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                return Enumerable.Empty<TEntity>();
            }
        }

        /// <summary>
        /// Retrieves a single entity from the database based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>The first matching entity if found; otherwise, null.</returns>
        public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _context.Set<TEntity>().FirstOrDefault(predicate)!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                return null!;
            }
        }

        /// <summary>
        /// Updates an existing entity in the database based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>The updated entity if successful; otherwise, null.</returns>
        public virtual TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity entity)
        {
            try
            {
                var entityToUpdate = _context.Set<TEntity>().FirstOrDefault(predicate);
                if (entityToUpdate != null)
                {
                    // Copy non-ID property values from the new entity to the existing entity
                    _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);

                    // Explicitly set the ID of the existing entity to prevent it from being modified
                    _context.Entry(entityToUpdate).Property("Id").IsModified = false;

                    // Save changes to persist the updated entity
                    _context.SaveChanges();
                }
                return entityToUpdate!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating entity: " + ex.Message);
                throw; // Rethrow the exception or handle it appropriately
            }
        }


        /// <summary>
        /// Deletes entities from the database based on the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities for deletion.</param>
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entityToDelete = _context.Set<TEntity>().FirstOrDefault(predicate);
                if (entityToDelete != null)
                {
                    _context.Set<TEntity>().Remove(entityToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                throw; // Rethrow the exception to propagate it
            }
        }

        /// <summary>
        /// Checks if any entities match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to check.</param>
        /// <returns>True if any entity matches the predicate; otherwise, false.</returns>
        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _context.Set<TEntity>().Any(predicate);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error :: " + ex.Message);
                return false;
            }
        }
    }
}
