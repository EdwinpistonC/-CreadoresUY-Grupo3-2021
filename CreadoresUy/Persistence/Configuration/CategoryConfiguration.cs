using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constant;
using Share.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // SQL config

            builder.ToTable("Category");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Description).IsRequired();


            //Seed Data

            builder.HasData(SeedDatabase());

        }


        public Collection<Category> SeedDatabase()
        {
            var categorys = new Collection<Category>();

            for (int i = 0; i < DataConstant.CategoryName.Count; i++)
            {

                string name;
                string description;

                categorys.Add(new Category
                {
                    Id=i+1,
                    Name = DataConstant.CategoryName[i],
                    Description = DataConstant.CategoryDescription[i]
                });



            }
            return categorys;
        }
    }
}
