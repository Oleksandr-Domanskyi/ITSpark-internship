using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Domain.Entity.Image;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationInfrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Product");

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CreatedBy")
                .HasColumnType("text");
        }
    }
}