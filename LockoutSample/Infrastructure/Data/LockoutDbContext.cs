using LockoutSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LockoutSample.Infrastructure.Data
{
    public class LockoutDbContext(DbContextOptions<LockoutDbContext> options) : DbContext(options)
    {
        public DbSet<LockCode> Codes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LockCode>(o =>
            {
                o.Property(code => code.LockoutEnd)
                    .HasDefaultValue(new DateTime(2000, 1, 1));
            });

            modelBuilder.Entity<LockCode>()
                .ToTable("Codes")
                .HasKey(o => o.UserId);
        }
    }
}
