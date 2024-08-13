using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Image;
using ApplicationCore.Domain.Entity.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationInfrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Image");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasColumnType("text");

            builder.Property(e => e.Path)
                .IsRequired()
                .HasColumnType("text");

            builder.HasKey(e => e.Id);

            builder.HasOne<Product>()
                .WithMany(p => p.images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}