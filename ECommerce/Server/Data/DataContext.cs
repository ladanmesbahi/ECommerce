namespace ECommerce.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Nineteen Eighty-Four",
                    Description = "This article is about the 1949 novel by George Orwell. For the year, see 1984. For other uses, see 1984 (disambiguation).",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/c/c3/1984first.jpg",
                    Price = 10.99m
                },
                new Product
                {
                    Id = 2,
                    Title = "Animal Farm",
                    Description = "It tells the story of a group of farm animals who rebel against their human farmer, hoping to create a society where the animals can be equal, free, and happy.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/8/85/Tt0204824.jpeg",
                    Price = 50m
                });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
    }
}
