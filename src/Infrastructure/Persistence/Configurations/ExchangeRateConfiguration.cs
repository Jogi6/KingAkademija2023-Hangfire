using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class ExchangeRateConfiguration: IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImeValute).HasMaxLength(64).IsRequired();
            builder.Property(x => x.KupovniTecaj).HasPrecision(18,9).IsRequired();
            builder.Property(x => x.SrednjiTecaj).HasPrecision(18, 9).IsRequired();
            builder.Property(x => x.Prodajni).HasPrecision(18, 9).IsRequired();

            //Auditable
            builder.Property(p => p.CreatedBy)
                .HasMaxLength(64)
                .IsUnicode()
                .IsRequired();

            builder.Property(p => p.LastModifiedBy)
                .HasMaxLength(64)
                .IsUnicode();

            builder.Property(p => p.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");

            builder.Property(p => p.LastModified)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getutcdate())");
        }
    }
}
