﻿using Amazon_eCommerce_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Amazon_eCommerce_API.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.StockQuantity);
            builder.Property(p => p.PictureUrl).IsRequired();

            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(t => t.ProductType).WithMany()
               .HasForeignKey(p => p.ProductTypeId);


            builder.HasOne(p => p.Category) // Relationship with Category
                .WithMany() // Assuming Category does not have a navigation property for Products
                .HasForeignKey(p => p.CategoryId); // Foreign key property


        }
    }
}
