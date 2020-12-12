using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Rocky.Models;

namespace Rocky.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<ApplicationType> ApplicationType { get; set; }

        public DbSet<Product> Product { get; set; }

    }
}