namespace BlazorEccomerce.Server.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
                
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductVariant>()
				.HasKey(p => new { p.ProductId, p.ProductTypeId });

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
						ProductId = 1,
						ProductTypeId = 1,
						Price = 1400.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductId = 1,
						ProductTypeId = 2,
						Price = 1500.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductId = 1,
						ProductTypeId = 3,
						Price = 1600.00m,
						OriginalPrice = 1600.00m
					},
					new ProductVariant
					{
						ProductId = 2,
						ProductTypeId = 1,
						Price = 1800.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductId = 2,
						ProductTypeId = 2,
						Price = 1900.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductId = 2,
						ProductTypeId = 3,
						Price = 2000.00m,
						OriginalPrice = 2000.00m
					},
					new ProductVariant
					{
						ProductId = 3,
						ProductTypeId = 4,
						Price = 4500.00m,
						OriginalPrice = 5000.00m
					},
					new ProductVariant
					{
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
				.HasOne(ci => ci.Product)
				.WithMany()
				.HasForeignKey(ci => ci.ProductId);

			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.ProductType)
				.WithMany()
				.HasForeignKey(ci => ci.ProductTypeId);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<ProductType> ProductTypes { get; set; }
		public DbSet<ProductVariant> ProductVariants { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
	}
}
