﻿using System.Linq.Expressions;
using AirBnB.Domain.Common.Query;
using AirBnB.Domain.Entities;

namespace AirBnB.Persistence.Repositories.Interfaces;

/// <summary>
/// Repository interface for managing listings.
/// </summary>
public interface IListingRepository
{
    /// <summary>
    /// Gets a queryable collection of listings based on the specified predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="asNoTracking"></param>
    /// <returns></returns>
    IQueryable<Listing> Get(Expression<Func<Listing, bool>>? predicate = default,
        bool asNoTracking = false);
    
    /// <summary>
    /// Gets a listing by its unique identifier asynchronously.
    /// </summary>
    /// <param name="listingId"></param>
    /// <param name="asNoTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Listing?> GetByIdAsync(Guid listingId, bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets listings by a collection of unique identifiers asynchronously.
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="asNoTracking"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<IList<Listing>> GetByIdsAsync(IEnumerable<Guid> ids, bool asNoTracking = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new listing asynchronously.
    /// </summary>
    /// <param name="listing"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Listing> CreateAsync(Listing listing, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing listing asynchronously.
    /// </summary>
    /// <param name="listing"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Listing> UpdateAsync(Listing listing, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing listing asynchronously.
    /// </summary>
    /// <param name="listing"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Listing?> DeleteAsync(Listing listing, bool saveChanges = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a listing by its unique identifier asynchronously.
    /// </summary>
    /// <param name="listingId"></param>
    /// <param name="saveChanges"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<Listing?> DeleteByIdAsync(Guid listingId, bool saveChanges = true,
        CancellationToken cancellationToken = default);
}