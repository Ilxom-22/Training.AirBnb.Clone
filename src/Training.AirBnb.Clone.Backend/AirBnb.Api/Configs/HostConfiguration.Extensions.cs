﻿using Backend_Project.Application.Entity;
using Backend_Project.Application.Listings.Services;
using Backend_Project.Application.Listings.Settings;
using Backend_Project.Application.Notifications.Services;
using Backend_Project.Application.Review.Settings;
using Backend_Project.Application.Validation;
using Backend_Project.Domain.Entities;
using Backend_Project.Infrastructure.CompositionServices;
using Backend_Project.Infrastructure.Services;
using Backend_Project.Infrastructure.Services.AccountServices;
using Backend_Project.Infrastructure.Services.ListingServices;
using Backend_Project.Infrastructure.Services.LocationServices;
using Backend_Project.Infrastructure.Services.NotificationsServices;
using Backend_Project.Infrastructure.Services.ReservationServices;
using Backend_Project.Infrastructure.Services.ReviewServices;
using Backend_Project.Persistence.DataContexts;
using Backend_Project.Persistence.SeedData;
using FileBaseContext.Context.Models.Configurations;
using System.Reflection;

namespace AirBnb.Api.Configs;

public static partial class HostConfiguration
{
    public static WebApplicationBuilder AddExposers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        return builder;
    }

    public static WebApplicationBuilder AddDevTools(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplicationBuilder AddDataContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDataContext, AppFileContext>(_ =>
        {
            var contextOptions = new FileContextOptions<AppFileContext>
            {
                StorageRootPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "DataStorage")
            };

            var context = new AppFileContext(contextOptions);
            context.FetchAsync().AsTask().Wait();

            return context;
        });

        return builder;
    }

    public static WebApplicationBuilder AddMapping(this WebApplicationBuilder builder)
    {
        var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).ToList();

        assemblies.Add(Assembly.GetExecutingAssembly());

        builder.Services.AddAutoMapper(assemblies);

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder
            .AddValidationServices()
            .AddAccountServices()
            .AddListingServices()
            .AddListingCategoryServices()
            .AddListingAmenityServices()
            .AddLocationServices()
            .AddReservationServices()
            .AddReviewServices()
            .AddNotificationServices();

        return builder;
    }

    public static WebApplication UseDevTools(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    public static async ValueTask<WebApplication> SeedDataAsync(this WebApplication app)
    {

        var context = app.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<IDataContext>();

        await context.InitializeUsersSeedDataAsync();
        await context.InitializeCategoryDetailsSeedData();
        await context.InitializeAmenityAndAmenityCategorySeedData();
        await context.InitializeListingPropertySeedData();
        await context.InitializeLocationSeedData();
        await context.InitializeEmailTemplateSeedDate();
        await context.InitializeAvailabilitySeedData();

        return app;
    }

    #region Registration of services divided into categories.

    private static WebApplicationBuilder AddAccountServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IEntityBaseService<User>, UserService>()
            .AddScoped<IEntityBaseService<UserCredentials>, UserCredentialsService>()
            .AddScoped<IEntityBaseService<PhoneNumber>, PhoneNumberService>();

        return builder;
    }

    private static WebApplicationBuilder AddListingServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ListingPropertyTypeSettings>(builder.Configuration.GetSection(nameof(ListingPropertyTypeSettings)));
        builder.Services.Configure<ListingSettings>(builder.Configuration.GetSection(nameof(ListingSettings)));
        builder.Services.Configure<ListingRulesSettings>(builder.Configuration.GetSection(nameof(ListingRulesSettings)));
        builder.Services.Configure<ListingRegistrationProgressSettings>(builder.Configuration.GetSection(nameof(ListingRegistrationProgressSettings)));


        builder.Services
            .AddScoped<IEntityBaseService<Listing>, ListingService>()
            .AddScoped<IEntityBaseService<ListingProperty>, ListingPropertyService>()
            .AddScoped<IEntityBaseService<ListingPropertyType>, ListingPropertyTypeService>()
            .AddScoped<IEntityBaseService<ListingRating>, ListingRatingService>()
            .AddScoped<IEntityBaseService<ListingRules>, ListingRulesService>()
            .AddScoped<IEntityBaseService<ListingRegistrationProgress>, ListingRegistrationProgressService>()
            .AddScoped<IListingRegistrationService, ListingRegistrationService>();

        return builder;
    }

    private static WebApplicationBuilder AddListingCategoryServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ListingTypeSettings>(builder.Configuration.GetSection(nameof(ListingTypeSettings)));

        builder.Services
            .AddScoped<IEntityBaseService<ListingCategory>, ListingCategoryService>()
            .AddScoped<IEntityBaseService<ListingFeature>, ListingFeatureService>()
            .AddScoped<IEntityBaseService<ListingType>, ListingTypeService>()
            .AddScoped<IEntityBaseService<ListingCategoryType>, ListingCategoryTypeService>()
            .AddScoped<IEntityBaseService<Description>, DescriptionService>()
            .AddScoped<IListingCategoryDetailsService, ListingCategoryDetailsService>();

        return builder;
    }

    private static WebApplicationBuilder AddListingAmenityServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IEntityBaseService<Amenity>, AmenityService>()
            .AddScoped<IEntityBaseService<AmenityCategory>, AmenityCategoryService>()
            .AddScoped<IEntityBaseService<ListingAmenities>, ListingAmenitiesService>()
            .AddScoped<IAmenitiesManagementService, AmenitiesManagementService>();

        return builder;
    }

    private static WebApplicationBuilder AddLocationServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IEntityBaseService<Address>, AddressService>()
            .AddScoped<IEntityBaseService<Country>, CountryService>()
            .AddScoped<IEntityBaseService<City>, CityService>();


        return builder;
    }

    private static WebApplicationBuilder AddReservationServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AvailabilitySettings>(builder.Configuration.GetSection(nameof(AvailabilitySettings)));

        builder.Services
            .AddScoped<IEntityBaseService<Reservation>, ReservationService>()
            .AddScoped<IEntityBaseService<ReservationOccupancy>, ReservationOccupancyService>()
            .AddScoped<IEntityBaseService<Availability>, AvailabilityService>();

        return builder;
    }

    private static WebApplicationBuilder AddReviewServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ReviewSettings>(builder.Configuration.GetSection(nameof(ReviewSettings)));

        builder.Services
            .AddScoped<IEntityBaseService<Comment>, CommentService>()
            .AddScoped<IEntityBaseService<Rating>, RatingService>();

        return builder;
    }

    private static WebApplicationBuilder AddNotificationServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IEntityBaseService<Email>, EmailService>()
            .AddScoped<IEntityBaseService<EmailTemplate>, EmailTemplateService>()
            .AddScoped<IEmailPlaceholderService, EmailPlaceholderService>()
            .AddScoped<IEmailSenderService, EmailSenderService>()
            .AddScoped<IEmailMessageService, EmailMessageSevice>()
            .AddScoped<IEmailManagementService, EmailManagementService>();

        return builder;
    }

    private static WebApplicationBuilder AddValidationServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IValidationService, ValidationService>();

        return builder;
    }

    #endregion
}