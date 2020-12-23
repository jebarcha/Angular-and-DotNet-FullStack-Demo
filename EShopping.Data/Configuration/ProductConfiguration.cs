using EShopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopping.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product", "musicStore");

            entity.Property(e => e.Name).HasMaxLength(256);

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        }
    }
}
