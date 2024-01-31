﻿// <auto-generated />
using System;
using AirBnB.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirBnB.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AirBnB.Domain.Entities.Listing", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateOnly>("BuiltDate")
                        .HasColumnType("date");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DeletedByUserId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.ListingCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSpecialCategory")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<Guid>("StorageFileId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("StorageFileId")
                        .IsUnique();

                    b.ToTable("ListingCategories");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.ListingCategoryAssociation", b =>
                {
                    b.Property<Guid>("ListingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ListingCategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasKey("ListingId", "ListingCategoryId");

                    b.HasIndex("ListingCategoryId");

                    b.ToTable("ListingCategoryAssociations", (string)null);
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.NotificationTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(129536)
                        .HasColumnType("character varying(129536)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TemplateType")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Type", "TemplateType")
                        .IsUnique();

                    b.ToTable("NotificationTemplates", (string)null);

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.StorageFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("StorageFiles");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEmailAddressVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.UserSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("ModifiedTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PreferredNotificationType")
                        .HasColumnType("integer");

                    b.Property<int>("PreferredTheme")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.VerificationCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("CodeType")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("ExpiryTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("VerificationLink")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.ToTable("VerificationCodes");

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.EmailTemplate", b =>
                {
                    b.HasBaseType("AirBnB.Domain.Entities.NotificationTemplate");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.SmsTemplate", b =>
                {
                    b.HasBaseType("AirBnB.Domain.Entities.NotificationTemplate");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.UserInfoVerificationCode", b =>
                {
                    b.HasBaseType("AirBnB.Domain.Entities.VerificationCode");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.Listing", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.User", "Host")
                        .WithMany("Listings")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("AirBnB.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("ListingId")
                                .HasColumnType("uuid");

                            b1.Property<string>("City")
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)");

                            b1.Property<Guid?>("CityId")
                                .HasColumnType("uuid");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.HasKey("ListingId");

                            b1.ToTable("Listings");

                            b1.WithOwner()
                                .HasForeignKey("ListingId");
                        });

                    b.OwnsOne("AirBnB.Domain.Entities.Money", "PricePerNight", b1 =>
                        {
                            b1.Property<Guid>("ListingId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric");

                            b1.Property<int>("Currency")
                                .HasColumnType("integer");

                            b1.HasKey("ListingId");

                            b1.ToTable("Listings");

                            b1.WithOwner()
                                .HasForeignKey("ListingId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Host");

                    b.Navigation("PricePerNight")
                        .IsRequired();
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.ListingCategory", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.StorageFile", "ImageStorageFile")
                        .WithOne()
                        .HasForeignKey("AirBnB.Domain.Entities.ListingCategory", "StorageFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageStorageFile");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.ListingCategoryAssociation", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.ListingCategory", "ListingCategory")
                        .WithMany()
                        .HasForeignKey("ListingCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AirBnB.Domain.Entities.Listing", "Listing")
                        .WithMany()
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Listing");

                    b.Navigation("ListingCategory");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.User", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.UserSettings", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("AirBnB.Domain.Entities.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.UserInfoVerificationCode", b =>
                {
                    b.HasOne("AirBnB.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AirBnB.Domain.Entities.User", b =>
                {
                    b.Navigation("Listings");

                    b.Navigation("UserSettings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
