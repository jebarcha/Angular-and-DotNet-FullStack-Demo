using EShopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShopping.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("User", "musicStore");

            entity.HasIndex(e => e.ProfileId, "IX_User_ProfileId");

            entity.Property(e => e.Email).HasMaxLength(100);

            entity.Property(e => e.LastName).HasMaxLength(256);

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(512);

            entity.Property(e => e.Username).HasMaxLength(25);

            entity.HasOne(d => d.Profile)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.ProfileId);
        }
    }
}
