using meoRazor.Models;
using Microsoft.EntityFrameworkCore;

namespace meoRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { categoryID = 1, categoryName = "meo", categoryOrder = 1 },
               new Category { categoryID = 2, categoryName = "neko", categoryOrder = 2 },
               new Category { categoryID = 3, categoryName = "meoneko", categoryOrder = 3 }
                );
        }
    }
}
