using Microsoft.EntityFrameworkCore;
using FourtitudeAspNet.Models;

namespace FourtitudeAspNet.Data
{
    public class FourtitudeDbContext : DbContext
    {
        public FourtitudeDbContext(DbContextOptions<FourtitudeDbContext> options) : base(options)
        {
        }

        public DbSet<Partner> Partners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        // Configure Partner entity
  modelBuilder.Entity<Partner>(entity =>
    {
    entity.HasKey(e => e.PartnerKey);
       entity.Property(e => e.PartnerKey).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PartnerName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Password).IsRequired().HasMaxLength(500);
       });

    // Seed initial data
   modelBuilder.Entity<Partner>().HasData(
                new Partner { PartnerKey = "FG-00001", PartnerName = "FAKEGOOGLE", Password = "FAKEPASSWORD1234" },
         new Partner { PartnerKey = "FG-00002", PartnerName = "FAKEPEOPLE", Password = "FAKEPASSWORD4578" }
      );
        }
}
}