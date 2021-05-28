using Microsoft.EntityFrameworkCore;
using los_api.Models;

namespace los_api.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext (DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}