﻿// <auto-generated />
using System;
using BlazorEccomerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorEccomerce.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BlazorEccomerce.Shared.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.CartItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CartId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ProductVariantId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Portrait Photoshoots",
                            Url = "portrait-photoshoots"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Couples Photoshoots",
                            Url = "couples-photoshoots"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Live Photoshoots",
                            Url = "live-photoshoots"
                        });
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.OrderItem", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductVariantId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OrderId", "ProductVariantId");

                    b.HasIndex("ProductVariantId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Featured")
                        .HasColumnType("bit");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Description = "One (1) hour photoshoot. This include between 80 to a 100 edited photos. This can be for portfolio, formals, social media, or just for the fun of it!",
                            Featured = false,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjUgaoL9tabSf-Ig21XIdTawnh-BgUEbQZWjFisvLr9hf3-ekVFntcXFCAHAlKoV24M6Q",
                            Title = "Personal Photoshoot"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Description = "Two (2) hours photoshoot. This include between 100 to 150 edited photos. This can be for special occasions, social media, or just for the love you share!",
                            Featured = true,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsNk9UUKvmqxEh2t17_5jCYBWO4e4mWK2sfw&s",
                            Title = "Couples Photoshoot"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Description = "Three (3) hours photoshoot. This include between 200 to 300 edited photos. This can be for events, live performances, or social media!",
                            Featured = false,
                            ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4JQhSxjiWp9Q3ClqBHpGwnoQI_MvLlmDph4xpBJq3N-I9zmsb1e-DJgPfEyupgMWgr9U",
                            Title = "Live Photography"
                        });
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Sunrise"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Midday"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Sunset"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Whole morning"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Whole afternoon"
                        });
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.ProductVariant", b =>
                {
                    b.Property<int>("ProductVariantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductVariantId"), 1L, 1);

                    b.Property<decimal>("OriginalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ProductTypeId")
                        .HasColumnType("int");

                    b.HasKey("ProductVariantId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("ProductVariants");

                    b.HasData(
                        new
                        {
                            ProductVariantId = 1,
                            OriginalPrice = 1600.00m,
                            Price = 1400.00m,
                            ProductId = 1,
                            ProductTypeId = 1
                        },
                        new
                        {
                            ProductVariantId = 2,
                            OriginalPrice = 1600.00m,
                            Price = 1500.00m,
                            ProductId = 1,
                            ProductTypeId = 2
                        },
                        new
                        {
                            ProductVariantId = 3,
                            OriginalPrice = 1600.00m,
                            Price = 1600.00m,
                            ProductId = 1,
                            ProductTypeId = 3
                        },
                        new
                        {
                            ProductVariantId = 4,
                            OriginalPrice = 2000.00m,
                            Price = 1800.00m,
                            ProductId = 2,
                            ProductTypeId = 1
                        },
                        new
                        {
                            ProductVariantId = 5,
                            OriginalPrice = 2000.00m,
                            Price = 1900.00m,
                            ProductId = 2,
                            ProductTypeId = 2
                        },
                        new
                        {
                            ProductVariantId = 6,
                            OriginalPrice = 2000.00m,
                            Price = 2000.00m,
                            ProductId = 2,
                            ProductTypeId = 3
                        },
                        new
                        {
                            ProductVariantId = 7,
                            OriginalPrice = 5000.00m,
                            Price = 4500.00m,
                            ProductId = 3,
                            ProductTypeId = 4
                        },
                        new
                        {
                            ProductVariantId = 8,
                            OriginalPrice = 5000.00m,
                            Price = 5000.00m,
                            ProductId = 3,
                            ProductTypeId = 5
                        });
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Cart", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.User", "User")
                        .WithOne("Cart")
                        .HasForeignKey("BlazorEccomerce.Shared.Cart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.CartItem", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.Cart", "Cart")
                        .WithMany("CartItems")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorEccomerce.Shared.ProductVariant", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Order", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.OrderItem", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorEccomerce.Shared.ProductVariant", "ProductVariant")
                        .WithMany()
                        .HasForeignKey("ProductVariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("ProductVariant");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Product", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.ProductVariant", b =>
                {
                    b.HasOne("BlazorEccomerce.Shared.Product", "Product")
                        .WithMany("Variants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorEccomerce.Shared.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Cart", b =>
                {
                    b.Navigation("CartItems");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.Product", b =>
                {
                    b.Navigation("Variants");
                });

            modelBuilder.Entity("BlazorEccomerce.Shared.User", b =>
                {
                    b.Navigation("Cart")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
