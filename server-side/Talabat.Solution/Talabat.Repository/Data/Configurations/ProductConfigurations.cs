using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Domain.Layer.Entities;

namespace Talabat.Repository.Layer.Data.Configrations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            
            builder.Property(p => p.Description).IsRequired();
            
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.Property(p => p.ImageUrl).IsRequired();

            builder.HasOne(p => p.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);

            builder.HasOne(p => p.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId);
        }
    }
}
