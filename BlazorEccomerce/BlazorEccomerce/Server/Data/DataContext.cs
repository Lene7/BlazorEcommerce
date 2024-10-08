﻿namespace BlazorEccomerce.Server.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
                
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductVariant>()
				//.HasKey(pv => pv.ProductVariantId)
				.HasOne(pv => pv.Product)
				.WithMany(p => p.Variants)
				.HasForeignKey(pv => pv.ProductId);

			modelBuilder.Entity<OrderItem>()
				.HasKey(oi => new {oi.OrderId, oi.ProductVariantId });

			modelBuilder.Entity<ProductType>().HasData(
					new ProductType { Id = 1, Name = "Sunrise"},
					new ProductType { Id = 2, Name = "Midday" },
					new ProductType { Id = 3, Name = "Sunset" },
					new ProductType { Id = 4, Name = "Whole morning" },
					new ProductType { Id = 5, Name = "Whole afternoon" }
				);

			modelBuilder.Entity<Category>().HasData(
				new Category
				{
					Id = 1,
					Name = "Portrait Photoshoots",
					Url = "portrait-photoshoots"
				},

				new Category
				{
					Id = 2,
					Name = "Couples Photoshoots",
					Url = "couples-photoshoots"
				},

				new Category
				{
					Id = 3,
					Name = "Live Photoshoots",
					Url = "live-photoshoots"
				}
			);

			modelBuilder.Entity<Product>().HasData(
					new Product
					{
						Id = 1,
						Title = "Personal Photoshoot",
						Description = "One (1) hour photoshoot. This include between 80 to a 100 edited photos. This can be for portfolio, formals, social media, or just for the fun of it!",
						ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjUgaoL9tabSf-Ig21XIdTawnh-BgUEbQZWjFisvLr9hf3-ekVFntcXFCAHAlKoV24M6Q",
						CategoryId = 1
					},

					new Product
					{
						Id = 2,
						Title = "Couples Photoshoot",
						Description = "Two (2) hours photoshoot. This include between 100 to 150 edited photos. This can be for special occasions, social media, or just for the love you share!",
						ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsNk9UUKvmqxEh2t17_5jCYBWO4e4mWK2sfw&s",
						CategoryId = 2,
						Featured = true
					},

					new Product
					{
						Id = 3,
						Title = "Live Photography",
						Description = "Three (3) hours photoshoot. This include between 200 to 300 edited photos. This can be for events, live performances, or social media!",
						ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4JQhSxjiWp9Q3ClqBHpGwnoQI_MvLlmDph4xpBJq3N-I9zmsb1e-DJgPfEyupgMWgr9U",
						CategoryId = 3
					}
			);

			modelBuilder.Entity<ProductVariant>().HasData(
					new ProductVariant
					{
						ProductVariantId = 1,
						ProductId = 1,
						ProductTypeId = 1,
						Price = 1400.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductVariantId = 2,
						ProductId = 1,
						ProductTypeId = 2,
						Price = 1500.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductVariantId = 3,
						ProductId = 1,
						ProductTypeId = 3,
						Price = 1600.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductVariantId = 4,
						ProductId = 2,
						ProductTypeId = 1,
						Price = 1800.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductVariantId = 5,
						ProductId = 2,
						ProductTypeId = 2,
						Price = 1900.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductVariantId = 6,
						ProductId = 2,
						ProductTypeId = 3,
						Price = 2000.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductVariantId = 7,
						ProductId = 3,
						ProductTypeId = 4,
						Price = 4500.00m,
						OriginalPrice = 5000.00m
					},
					new ProductVariant
					{
						ProductVariantId = 8,
						ProductId = 3,
						ProductTypeId = 5,
						Price = 5000.00m,
						OriginalPrice = 5000.00m
					}
				);

			modelBuilder.Entity<User>()
			   .HasOne(u => u.Cart)
			   .WithOne(c => c.User)
			   .HasForeignKey<Cart>(c => c.UserId);

			modelBuilder.Entity<Cart>()
				.HasMany(c => c.CartItems)
				.WithOne(ci => ci.Cart)
				.HasForeignKey(ci => ci.CartId);

			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.ProductVariant)
				.WithMany()
				.HasForeignKey(ci => ci.ProductVariantId);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductType> ProductTypes { get; set; }
		public DbSet<ProductVariant> ProductVariants { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Image> Images { get; set; }
	}
}
