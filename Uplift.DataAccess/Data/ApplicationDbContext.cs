using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uplift.Models;

namespace Uplift.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Frequency> Frequency { get; set; }

        public DbSet<Service> Service { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }
    }
}
