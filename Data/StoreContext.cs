using Microsoft.EntityFrameworkCore;
using los_api.Models;

namespace los_api.Data
{
  public class StoreContext : DbContext
  {
    public StoreContext(DbContextOptions<StoreContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
    public DbSet<Stock> Stock { get; set; }
  }
}