using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MaturityBenefitProc.Repositories
{
    /// <summary>
    /// Generic repository interface for CRUD operations on domain entities.
    /// Provides a consistent data access abstraction for the maturity benefit processing system.
    /// </summary>
    /// <typeparam name="T">The entity type managed by this repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves an entity by its primary key identifier.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        /// <returns>The entity if found; otherwise null.</returns>
        T GetById(int id);

        /// <summary>
        /// Retrieves all entities of this type.
        /// </summary>
        /// <returns>A collection of all entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Retrieves entities matching the specified predicate.
        /// </summary>
        /// <param name="predicate">A filter expression to apply.</param>
        /// <returns>A collection of matching entities.</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a single entity matching the predicate, or null if none found.
        /// Throws if more than one entity matches.
        /// </summary>
        /// <param name="predicate">A filter expression to apply.</param>
        /// <returns>The matching entity or null.</returns>
        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Adds a collection of entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        void Update(T entity);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);

        /// <summary>
        /// Removes a collection of entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Returns the count of entities matching the predicate.
        /// </summary>
        /// <param name="predicate">A filter expression to apply.</param>
        /// <returns>The number of matching entities.</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Checks whether any entity matches the predicate.
        /// </summary>
        /// <param name="predicate">A filter expression to apply.</param>
        /// <returns>True if at least one entity matches; otherwise false.</returns>
        bool Any(Expression<Func<T, bool>> predicate);
    }
}
