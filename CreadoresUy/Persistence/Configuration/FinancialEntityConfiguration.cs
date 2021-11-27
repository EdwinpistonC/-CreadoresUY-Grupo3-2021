using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    public class FinancialEntityConfiguration : IEntityTypeConfiguration<FinancialEntity>
    {

        public void Configure(EntityTypeBuilder<FinancialEntity> builder)
        {
            builder.ToTable("FinancialEntity");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.RUT).IsRequired().HasMaxLength(12);
            builder.Property(f => f.Name).IsRequired().HasMaxLength(450);
            builder.Property(f => f.Country).IsRequired().HasMaxLength(450);
            builder.Property(f => f.Phone).IsRequired().HasMaxLength(12);

            builder.HasData(SeedDatabase());
        }
        public Collection<FinancialEntity> SeedDatabase()
        {
            var Entidades = new Collection<FinancialEntity>();

            var e1 = new FinancialEntity(147523001,"BROU","URY", 08001234);
            var e2 = new FinancialEntity(258634112,"ITAU", "URY", 08002345);
            var e3 = new FinancialEntity(369745223,"SANTANDER", "URY", 08003456);
            var e4 = new FinancialEntity(470856334,"BBVA", "URY", 08004567);
            var e5 = new FinancialEntity(581967445,"SCOTIABANK", "URY", 08005678);
            e1.Id = 1; e2.Id = 2; e3.Id = 3; e4.Id = 4; e5.Id = 5;

            Entidades.Add(e1);
            Entidades.Add(e2);
            Entidades.Add(e3);
            Entidades.Add(e4);
            Entidades.Add(e5);
            return Entidades;
        }
    }
}
