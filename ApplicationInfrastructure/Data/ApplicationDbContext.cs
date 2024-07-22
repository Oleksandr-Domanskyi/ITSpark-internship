using ApplicationCore.Domain.Authorization;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Entity.ItemProfile;
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

        public DbSet<ItemProfile> ItemProfile { get; set; }
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

            modelBuilder.Entity<ItemProfile>()
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
                entity.HasOne<ItemProfile>()
                    .WithMany(p => p.images)
                    .HasForeignKey(i => i.ItemProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
