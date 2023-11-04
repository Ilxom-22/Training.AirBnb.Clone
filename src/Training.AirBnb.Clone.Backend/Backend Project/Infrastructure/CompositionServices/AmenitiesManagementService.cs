using Backend_Project.Application.Foundations.ListingServices;
using Backend_Project.Application.Listings.Services;
using Backend_Project.Domain.Entities;
using Backend_Project.Domain.Exceptions.EntityExceptions;

namespace Backend_Project.Infrastructure.CompositionServices
{
    public class AmenitiesManagementService : IAmenitiesManagementService
    {
        private readonly IAmenityService _amenityService;
        private readonly IAmenityCategoryService _amenityCategoryService;
        private readonly IListingAmenitiesService _listingAmenitiesService;

        public AmenitiesManagementService(IAmenityService amenityService,
            IAmenityCategoryService amenityCategoryService,
            IListingAmenitiesService listingAmenitiesService)
        {
            _amenityService = amenityService;
            _amenityCategoryService = amenityCategoryService;
            _listingAmenitiesService = listingAmenitiesService;
        }

#region Amenitie's methods
        public async ValueTask<Amenity> AddAmenity(Amenity amenity, bool saveChanges = true, 
            CancellationToken cancellationToken = default)
        {
            await _amenityCategoryService.GetByIdAsync(amenity.CategoryId, cancellationToken);

            return await _amenityService.CreateAsync(amenity, saveChanges, cancellationToken);
        }

        public async ValueTask<Amenity> UpdateAmenityAsycn(Amenity amenity, bool saveChanges = true,
            CancellationToken cancellationToken = default)
        {
            await _amenityCategoryService.GetByIdAsync(amenity.CategoryId, cancellationToken);
            
            return await _amenityService.UpdateAsync(amenity, saveChanges, cancellationToken);
        }
        
        public async ValueTask<Amenity> DeleteAmenityAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var amenity = await _amenityService.GetByIdAsync(id, cancellationToken);

            var listingAmenities =  _listingAmenitiesService
                .Get(la => la.AmenityId.Equals(id));

            if (listingAmenities.Any())
                throw new EntityNotDeletableException<Amenity>("this amenity not Deletable");

            return await _amenityService.DeleteAsync(amenity, saveChanges, cancellationToken);
        }

        #endregion

        // AmenitiesCategorie's methods
        public ValueTask<ICollection<Amenity>> GetAmenitiesByCategoryId( Guid amenityCategoryId, CancellationToken cancellationToken = default)
                =>  new (
                 _amenityService.Get(ac => ac.CategoryId.Equals(amenityCategoryId)).ToList());

        public async ValueTask<AmenityCategory> DeleteAmenitiesCategory(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            var amenitiesCategory = await _amenityCategoryService.GetByIdAsync(id, cancellationToken);

            var amenities = _amenityService.Get(a => a.CategoryId.Equals(amenitiesCategory.Id));

            if (amenities.Any())
                throw new EntityNotDeletableException<AmenityCategory>("This Category not Deletable");

            return await _amenityCategoryService.DeleteAsync(amenitiesCategory, saveChanges, cancellationToken);
        }


        // ListingAmenites methods 
        public async ValueTask<ListingAmenities> AddListingAmenitiesAsync(ListingAmenities listingAmenities,
            bool saveChanges = true, CancellationToken cancellationToken = default)
        {
            await _amenityService.GetByIdAsync(listingAmenities.AmenityId, cancellationToken);

            return await _listingAmenitiesService.CreateAsync(listingAmenities, saveChanges, cancellationToken);
        }
    }
}