﻿// <auto-generated />
using BlazorEccomerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorEccomerce.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240705140701_ProductSeeding")]
    partial class ProductSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlazorEccomerce.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "One (1) hour photoshoot. This include between 80 to a 100 edited photos. This can be for portfolio, formals, social media, or just for the fun of it!",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjUgaoL9tabSf-Ig21XIdTawnh-BgUEbQZWjFisvLr9hf3-ekVFntcXFCAHAlKoV24M6Q",
                            Price = 1600.00m,
                            Title = "Personal Photoshoot"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Two (2) hours photoshoot. This include between 100 to 150 edited photos. This can be for special occasions, social media, or just for the love you share!",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsNk9UUKvmqxEh2t17_5jCYBWO4e4mWK2sfw&s",
                            Price = 2000.00m,
                            Title = "Couples Photoshoot"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Three (3) hours photoshoot. This include between 200 to 300 edited photos. This can be for events, live performances, or social media!",
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4JQhSxjiWp9Q3ClqBHpGwnoQI_MvLlmDph4xpBJq3N-I9zmsb1e-DJgPfEyupgMWgr9U",
                            Price = 5000.00m,
                            Title = "Live Photography"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
