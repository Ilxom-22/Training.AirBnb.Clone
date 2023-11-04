﻿using Backend_Project.Application.Entity;
using Backend_Project.Application.Listings.Settings;
using Backend_Project.Domain.Entities;
using Backend_Project.Domain.Exceptions.EntityExceptions;
using Backend_Project.Persistence.DataContexts;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Backend_Project.Infrastructure.Services.ListingServices;

public class ListingService : IEntityBaseService<Listing>
{ 
    private readonly IDataContext _appDataContext;
    private readonly ListingSettings _listingSettings;

    public ListingService(IDataContext appDataContext, IOptions<ListingSettings> listingSettings)
    {
        _appDataContext = appDataContext;
        _listingSettings = listingSettings.Value;
    }

    public async ValueTask<Listing> CreateAsync(Listing listing, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (!IsValidListing(listing))
            throw new EntityValidationException<Listing>("Listing did not pass validation.");

        await _appDataContext.Listings.AddAsync(listing, cancellationToken);

        if (saveChanges) await _appDataContext.SaveChangesAsync();

        return listing;
    }

    public ValueTask<ICollection<Listing>> GetAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => new ValueTask<ICollection<Listing>>(GetUndeletedListings()
            .Where(listing => ids.Contains(listing.Id))
            .ToList());

    public ValueTask<Listing> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => new ValueTask<Listing>(GetUndeletedListings()
                .FirstOrDefault(listing => listing.Id == id)
                ?? throw new EntityNotFoundException<Listing> ("Listing not found."));

    public IQueryable<Listing> Get(Expression<Func<Listing, bool>> predicate)
        => GetUndeletedListings().Where(predicate.Compile()).AsQueryable();

    public async ValueTask<Listing> UpdateAsync(Listing listing, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (!IsValidListing(listing))
            throw new EntityValidationException<Listing>("Listing did not pass validation.");

        var foundListing = await GetByIdAsync(listing.Id, cancellationToken);

        foundListing.Title = listing.Title;
        foundListing.Status = listing.Status;
        foundListing.Price = listing.Price;
        
        await _appDataContext.Listings.UpdateAsync(foundListing, cancellationToken);
        
        if (saveChanges) await _appDataContext.SaveChangesAsync();

        return listing;
    }

    public async ValueTask<Listing> DeleteAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundListing = await GetByIdAsync(id, cancellationToken);

        await _appDataContext.Listings.RemoveAsync(foundListing, cancellationToken);

        if (saveChanges) await _appDataContext.SaveChangesAsync();

        return foundListing;
    }

    public async ValueTask<Listing> DeleteAsync(Listing listing, bool saveChanges = true, CancellationToken cancellationToken = default)
        => await DeleteAsync(listing.Id, saveChanges, cancellationToken);

    private bool IsValidListing(Listing listing)
    {
        if (string.IsNullOrWhiteSpace(listing.Title) || listing.Title.Length < _listingSettings.MinTitleLength || listing.Title.Length > _listingSettings.MaxTitleLength) 
            return false;

        if (listing.Price is not null && listing.Price < _listingSettings.MinListingPrice)
            return false;

        return true;
    }

    private IQueryable<Listing> GetUndeletedListings()
        => _appDataContext.Listings.Where(listing => !listing.IsDeleted).AsQueryable();
}