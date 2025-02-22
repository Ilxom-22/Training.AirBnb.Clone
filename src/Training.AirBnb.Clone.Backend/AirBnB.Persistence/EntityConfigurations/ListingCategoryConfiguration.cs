﻿using AirBnB.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirBnB.Persistence.EntityConfigurations;

public class ListingCategoryConfiguration : IEntityTypeConfiguration<ListingCategory>
{
    public void Configure(EntityTypeBuilder<ListingCategory> builder)
    {
        builder
            .Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .Property(category => category.StorageFileId)
            .IsRequired();

        builder
            .HasIndex(category => category.Name)
            .IsUnique();

        builder
            .HasOne(category => category.ImageStorageFile)
            .WithOne()
            .HasForeignKey<ListingCategory>(category => category.StorageFileId);
        
        builder.HasMany(category => category.Listings)
            .WithMany(listing => listing.ListingCategories)
            .UsingEntity<ListingCategoryAssociation>(
                categoryAssociation =>
                {
                    categoryAssociation.HasKey(relation => new { relation.ListingId, relation.ListingCategoryId });
                    
                    categoryAssociation.ToTable("ListingCategoryAssociations");
                }
            );
    }
}