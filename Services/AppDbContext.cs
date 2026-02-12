using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WidgetService.Models;

namespace WidgetService.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<WidgetModel> Widget => Set<WidgetModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WidgetModel>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd(); // ensures identity generation

            modelBuilder.Entity<ApplicationUser>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd(); // ensures identity generation
        }
    }
}