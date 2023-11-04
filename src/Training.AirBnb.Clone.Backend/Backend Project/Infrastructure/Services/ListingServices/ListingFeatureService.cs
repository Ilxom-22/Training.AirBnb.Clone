using Backend_Project.Application.Foundations.ListingServices;
using Backend_Project.Application.Listings.Settings;
using Backend_Project.Domain.Entities;
using Backend_Project.Domain.Exceptions.EntityExceptions;
using Backend_Project.Persistence.DataContexts;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Backend_Project.Infrastructure.Services.ListingServices;

public class ListingFeatureService : IListingFeatureService
{
    private readonly IDataContext _appDataContext;
    private readonly ListingRulesSettings _featureSettings;

    public ListingFeatureService(IDataContext appDataContext, IOptions<ListingRulesSettings> featureSettings)
    {
        _appDataContext = appDataContext;
        _featureSettings = featureSettings.Value;
    }

    public async ValueTask<ListingFeature> CreateAsync(ListingFeature feature, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        ValidateOnCreate(feature);

        await _appDataContext.ListingFeatures.AddAsync(feature, cancellationToken);

        if (saveChanges) await _appDataContext.SaveChangesAsync();

        return feature;
    }

    public ValueTask<ICollection<ListingFeature>> GetAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        => new (GetUndeletedFeatures()
                .Where(feature => ids.Contains(feature.Id))
                .ToList());

    public ValueTask<ListingFeature> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => new (GetUndeletedFeatures()
            .FirstOrDefault(feature => feature.Id == id)
            ?? throw new EntityNotFoundException<ListingFeature> ("Listing Feature not found!"));

    public IQueryable<ListingFeature> Get(Expression<Func<ListingFeature, bool>> predicate)
        => GetUndeletedFeatures().Where(predicate.Compile()).AsQueryable();

    public async ValueTask<ListingFeature> UpdateAsync(ListingFeature feature, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundFeature = await GetByIdAsync(feature.Id, cancellationToken);

        if (!ValidateOnUpdate(feature, foundFeature))
            throw new EntityValidationException<ListingFeature>("Not valid listing feature.");

        foundFeature.Name = feature.Name;
        foundFeature.MinValue = feature.MinValue;
        foundFeature.MaxValue = feature.MaxValue;   
        foundFeature.ListingTypeId = feature.ListingTypeId;

        await _appDataContext.ListingFeatures.UpdateAsync(foundFeature, cancellationToken);

        if (saveChanges) await _appDataContext.SaveChangesAsync();

        return foundFeature;
    }

    public async ValueTask<ListingFeature> DeleteAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundFeature = await GetByIdAsync(id, cancellationToken);

        await _appDataContext.ListingFeatures.RemoveAsync(foundFeature, cancellationToken);

        if (saveChanges) await _appDataContext.ListingFeatures.SaveChangesAsync(cancellationToken);

        return foundFeature;
    }

    public async ValueTask<ListingFeature> DeleteAsync(ListingFeature feature, bool saveChanges = true, CancellationToken cancellationToken = default)
        => await DeleteAsync(feature.Id, saveChanges, cancellationToken);

    private void ValidateOnCreate(ListingFeature feature)
    {
        if (!IsValidFeature(feature))
            throw new EntityValidationException<ListingFeature>("Not valid feature!");

        if (FeatureExists(feature))
            throw new DuplicateEntityException<ListingFeature>("Feature already exists!");
    }

    private bool ValidateOnUpdate(ListingFeature feature, ListingFeature existingFeature)
    {
        if (existingFeature.Name == feature.Name && existingFeature.ListingTypeId == feature.ListingTypeId)
            return IsValidFeature(feature);

        if (FeatureExists(feature))
            throw new DuplicateEntityException<ListingFeature>("Listing feature already exists.");

        return true;
    }

    private bool IsValidFeature(ListingFeature feature)
        => !string.IsNullOrWhiteSpace(feature.Name)
            && feature.MinValue >= _featureSettings.FeatureMinValue;

    private bool FeatureExists(ListingFeature feature)
        => GetUndeletedFeatures()
            .Any(self => self.Name == feature.Name 
                && self.ListingTypeId == feature.ListingTypeId);

    private IQueryable<ListingFeature> GetUndeletedFeatures()
        => _appDataContext.ListingFeatures.Where(feature => !feature.IsDeleted).AsQueryable();
}