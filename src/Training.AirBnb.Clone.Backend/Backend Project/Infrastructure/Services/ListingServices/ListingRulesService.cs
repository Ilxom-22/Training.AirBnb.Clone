﻿using Backend_Project.Application.Entity;
using Backend_Project.Application.Listings.Settings;
using Backend_Project.Domain.Entities;
using Backend_Project.Domain.Exceptions.EntityExceptions;
using Backend_Project.Persistence.DataContexts;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Backend_Project.Infrastructure.Services.ListingServices;

public class ListingRulesService : IEntityBaseService<ListingRules>
{
    private readonly IDataContext _context;
    private readonly ListingRulesSettings _rulesSettings;

    public ListingRulesService(IDataContext context, IOptions<ListingRulesSettings> rulesSettings)
    {
        _context = context;
        _rulesSettings = rulesSettings.Value;
    }

    public async ValueTask<ListingRules> CreateAsync(ListingRules listingRules, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        Validate(listingRules);

        await _context.ListingRules.AddAsync(listingRules, cancellationToken);

        if (saveChanges)
            await _context.SaveChangesAsync();

        return listingRules;

    }

    public async ValueTask<ListingRules> DeleteAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundListingRules = await _context.ListingRules.FindAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException<ListingRules>("Listing rules not found!");

        await _context.ListingRules.RemoveAsync(foundListingRules, cancellationToken);

        if (saveChanges)
            await _context.SaveChangesAsync();

        return foundListingRules;
    }

    public ValueTask<ListingRules> DeleteAsync(ListingRules listingRules, bool saveChanges = true, CancellationToken cancellationToken = default)
        => DeleteAsync(listingRules.Id, saveChanges, cancellationToken);

    public IQueryable<ListingRules> Get(Expression<Func<ListingRules, bool>> predicate)
        => _context.ListingRules.Where(predicate.Compile()).AsQueryable();

    public ValueTask<ICollection<ListingRules>> GetAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => new ValueTask<ICollection<ListingRules>>(_context.ListingRules.Where(listingRules => ids.Contains(listingRules.Id)).ToList());

    public async ValueTask<ListingRules> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ListingRules.FindAsync(id, cancellationToken)
            ?? throw new EntityNotFoundException<ListingRules>("Listing rules not found with this id");

    public async ValueTask<ListingRules> UpdateAsync(ListingRules listingRules, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundListingRules = await GetByIdAsync(listingRules.Id, cancellationToken)
            ?? throw new EntityNotFoundException<ListingRules>("ListingRules not found with this id!");

        Validate(listingRules);

        foundListingRules.Guests = listingRules.Guests;

        foundListingRules.PetsAllowed = listingRules.PetsAllowed;

        foundListingRules.EventsAllowed = listingRules.EventsAllowed;

        foundListingRules.SmokingAllowed = listingRules.SmokingAllowed;

        foundListingRules.CommercialFilmingAllowed = listingRules.CommercialFilmingAllowed;

        foundListingRules.CheckInTimeStart = listingRules.CheckInTimeStart;

        foundListingRules.CheckInTimeEnd = listingRules.CheckInTimeEnd;

        foundListingRules.CheckOutTime = listingRules.CheckOutTime;

        foundListingRules.AdditionalRules = listingRules.AdditionalRules;

        await _context.ListingRules.UpdateAsync(foundListingRules, cancellationToken);

        if (saveChanges)
            await _context.SaveChangesAsync();

        return foundListingRules;

    }

    private void Validate(ListingRules listingRules)
    {
        if (listingRules.Guests < _rulesSettings.GuestsMinCount)
        {
            throw new EntityValidationException<ListingRules>("Guests count isn't valid!");
        }

        if (listingRules.CheckInTimeStart is null && listingRules.CheckInTimeEnd is not null)
        {
            throw new EntityValidationException<ListingRules>("Check-in end time must be left unspecified when the check-in start time is missing or null.");
        }

        if (listingRules.CheckInTimeStart is not null
            && (listingRules.CheckInTimeEnd is null
            || (listingRules.CheckInTimeEnd - listingRules.CheckInTimeStart) < TimeSpan.FromHours(_rulesSettings.MinCheckInDurationInHours)))
        {
            throw new EntityValidationException<ListingRules>("Invalid 'CheckInTimeStart' or 'CheckInTimeEnd'");
        }

        if (listingRules.AdditionalRules is not null && listingRules.AdditionalRules.All(@char => @char == ' '))
        {
            listingRules.AdditionalRules = null;
        }
    }
}