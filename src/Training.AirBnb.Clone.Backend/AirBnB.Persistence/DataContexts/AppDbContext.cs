using AirBnB.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirBnB.Persistence.DataContexts;

/// <summary>
/// Represents Database Context for the entire web application
/// </summary>
/// <param name="options"></param>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region Identity
    
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    public DbSet<UserCredentials> UserCredentialss => Set<UserCredentials>();

    #endregion
    #region Notification

    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();

    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();

    public DbSet<SmsTemplate> SmsTemplates => Set<SmsTemplate>();

    public DbSet<VerificationCode> VerificationCodes => Set<VerificationCode>();

    public DbSet<UserInfoVerificationCode> UserInfoVerificationCodes => Set<UserInfoVerificationCode>();

    #endregion
    #region Media

    public DbSet<StorageFile> StorageFiles => Set<StorageFile>();

    #endregion
    #region Listings

    public DbSet<Listing> Listings => Set<Listing>();
    
    public DbSet<ListingCategory> ListingCategories => Set<ListingCategory>();

    public DbSet<ListingCategoryAssociation> ListingCategoryAssociations => Set<ListingCategoryAssociation>();

    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}