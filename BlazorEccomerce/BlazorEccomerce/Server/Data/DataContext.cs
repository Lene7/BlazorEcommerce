﻿namespace BlazorEccomerce.Server.Data
{
	public class DataContext : DbContext
	{
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
                
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
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
						Price = 1600.00m,
						CategoryId = 1
					},

					new Product
					{
						Id = 2,
						Title = "Couples Photoshoot",
						Description = "Two (2) hours photoshoot. This include between 100 to 150 edited photos. This can be for special occasions, social media, or just for the love you share!",
						ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsNk9UUKvmqxEh2t17_5jCYBWO4e4mWK2sfw&s",
						Price = 2000.00m,
						CategoryId = 2
					},

					new Product
					{
						Id = 3,
						Title = "Live Photography",
						Description = "Three (3) hours photoshoot. This include between 200 to 300 edited photos. This can be for events, live performances, or social media!",
						ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4JQhSxjiWp9Q3ClqBHpGwnoQI_MvLlmDph4xpBJq3N-I9zmsb1e-DJgPfEyupgMWgr9U",
						Price = 5000.00m,
						CategoryId = 3
					}
				);
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}
