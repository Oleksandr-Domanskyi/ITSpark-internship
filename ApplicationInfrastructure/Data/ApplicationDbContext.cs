using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Entity.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApplicationInfrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Product> ItemProfile { get; set; }
        public DbSet<Image> Image { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("identity");

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.SetPropertyAccessMode(PropertyAccessMode.Field);
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Product>()
            .Property(p => p.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasColumnType("text");

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image", "identity");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CreatedBy")
                    .HasColumnType("text");
                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("text");
                entity.HasKey(e => e.Id);
                entity.HasOne<Product>()
                    .WithMany(p => p.images)
                    .HasForeignKey(i => i.ItemProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
