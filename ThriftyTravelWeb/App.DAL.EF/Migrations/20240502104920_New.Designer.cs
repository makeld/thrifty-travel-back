﻿// <auto-generated />
using System;
using App.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace App.DAL.EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240502104920_New")]
    partial class New
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("App.Domain.Identity.AppRefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpirationDT")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("PreviousExpirationDT")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PreviousRefreshToken")
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentCommentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ParentCommentId");

                    b.HasIndex("TripId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Domain.Entities.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpenseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ExpensePrice")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripLocationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TripId");

                    b.HasIndex("TripLocationId");

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("TripId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Domain.Entities.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Domain.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid?>("ExpenseId")
                        .HasColumnType("uuid");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("TripId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Domain.Entities.Trip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Domain.Entities.TripCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("TripId");

                    b.ToTable("TripCategories");
                });

            modelBuilder.Entity("Domain.Entities.TripLocation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("LocationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("TripId");

                    b.ToTable("TripLocations");
                });

            modelBuilder.Entity("Domain.Entities.TripUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("TripId");

                    b.ToTable("TripUsers");
                });

            modelBuilder.Entity("Domain.Entities.UserExpense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExpenseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TripUserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseId");

                    b.HasIndex("TripUserId");

                    b.ToTable("UserExpenses");
                });

            modelBuilder.Entity("Domain.Identity.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Domain.Identity.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Domain.Identity.AppUserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("App.Domain.Identity.AppRefreshToken", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", "AppUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", "AppUser")
                        .WithMany("Comments")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Comment", "ParentComment")
                        .WithMany("ChildComments")
                        .HasForeignKey("ParentCommentId");

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("Comments")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("ParentComment");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.Expense", b =>
                {
                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("Expenses")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.TripLocation", "TripLocation")
                        .WithMany("Expenses")
                        .HasForeignKey("TripLocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("TripLocation");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", "AppUser")
                        .WithMany("Likes")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("Likes")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.Location", b =>
                {
                    b.HasOne("Domain.Entities.Country", "Country")
                        .WithMany("Locations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Domain.Entities.Photo", b =>
                {
                    b.HasOne("Domain.Entities.Expense", "Expense")
                        .WithMany("Photos")
                        .HasForeignKey("ExpenseId");

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("Photos")
                        .HasForeignKey("TripId");

                    b.Navigation("Expense");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.TripCategory", b =>
                {
                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("TripCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("TripCategories")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.TripLocation", b =>
                {
                    b.HasOne("Domain.Entities.Location", "Location")
                        .WithMany("TripLocations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("TripLocations")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.TripUser", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", "AppUser")
                        .WithMany("TripUsers")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Trip", "Trip")
                        .WithMany("TripUsers")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Domain.Entities.UserExpense", b =>
                {
                    b.HasOne("Domain.Entities.Expense", "Expense")
                        .WithMany("UserExpenses")
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.TripUser", "TripUser")
                        .WithMany("UserExpenses")
                        .HasForeignKey("TripUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Expense");

                    b.Navigation("TripUser");
                });

            modelBuilder.Entity("Domain.Identity.AppUserRole", b =>
                {
                    b.HasOne("Domain.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Domain.Identity.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("TripCategories");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Navigation("ChildComments");
                });

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Navigation("Locations");
                });

            modelBuilder.Entity("Domain.Entities.Expense", b =>
                {
                    b.Navigation("Photos");

                    b.Navigation("UserExpenses");
                });

            modelBuilder.Entity("Domain.Entities.Location", b =>
                {
                    b.Navigation("TripLocations");
                });

            modelBuilder.Entity("Domain.Entities.Trip", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Expenses");

                    b.Navigation("Likes");

                    b.Navigation("Photos");

                    b.Navigation("TripCategories");

                    b.Navigation("TripLocations");

                    b.Navigation("TripUsers");
                });

            modelBuilder.Entity("Domain.Entities.TripLocation", b =>
                {
                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("Domain.Entities.TripUser", b =>
                {
                    b.Navigation("UserExpenses");
                });

            modelBuilder.Entity("Domain.Identity.AppUser", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("RefreshTokens");

                    b.Navigation("TripUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
