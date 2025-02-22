﻿using System.Linq.Expressions;
using AirBnB.Application.Listings.Models;
using AirBnB.Domain.Common.Query;
using AirBnB.Domain.Entities;

namespace AirBnB.Application.Listings.Services;

/// <summary>
/// Defines location category foundation service functionalities.
/// </summary>
public interface IListingCategoryService
{
    /// <summary>
    /// Retrieves a queryable collection of ListingCategory entities based on the specified predicate.
    /// </summary>
    /// <param name="predicate">A predicate to filter the ListingCategory entities (optional).</param>
    /// <param name="asNoTracking">Indicates whether to disable change tracking for the entities (default: false).</param>
    /// <returns>A queryable collection of ListingCategory entities.</returns>
    IQueryable<ListingCategory> Get(Expression<Func<ListingCategory, bool>>? predicate = default,
        bool asNoTracking = false);

    IQueryable<ListingCategory> Get(ListingCategoryFilter listingCategoryFilter, bool asNoTracking);
}