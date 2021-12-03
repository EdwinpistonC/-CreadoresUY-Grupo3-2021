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
    class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(30);
            builder.HasData(SeedDatabase());
        }
        public Collection<Category> SeedDatabase()
        {
            var Entidades = new Collection<Category>();
            var c = new Category("Arte"); c.Id = 1;
            var c1 = new Category("Musica"); c1.Id = 2;
            var c2 = new Category("Trading"); c2.Id = 3;
            var c3 = new Category("Comida"); c3.Id = 4;
            Entidades.Add(c);
            Entidades.Add(c1);
            Entidades.Add(c2);
            Entidades.Add(c3);
            return Entidades;
        }
    }
}
