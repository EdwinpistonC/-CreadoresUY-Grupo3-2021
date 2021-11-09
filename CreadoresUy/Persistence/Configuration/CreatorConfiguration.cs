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
    class CreatorConfiguration : IEntityTypeConfiguration<Creator>
    {

        public void Configure(EntityTypeBuilder<Creator> builder)
        {
            // SQL config

            builder.ToTable("Creator");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.CreatorName).IsRequired();
            builder.Property(e => e.NickName).IsRequired().HasMaxLength(450);
            builder.Property(e => e.ContentDescription).IsRequired();
            builder.HasIndex(e => e.CreatorName).IsUnique();

            builder.HasOne<User>(c => c.User)
            .WithOne(u => u.Creator)
            .HasForeignKey<User>(u => u.CreatorId);




        }


        public static Collection<Creator> SeedDatabase(EntityTypeBuilder<Creator> builder)
        {


            var creadores = new Collection<Creator>();

            var datas = new DataConstant();
            Random r = new Random();

            var planId = 1;

            for (int i = 0; i < DataConstant.CreatorQuantity; i++)
            {
                var name = datas.Names[r.Next(0, datas.Names.Count)];
                Creator creator = new Creator { Id = i + 1, ContentDescription = name, NickName = name, CreatorName = String.Concat(name, "Creator") };

                int cantPlan = r.Next(1, r.Next(3, DataConstant.MaxPlans));
                ICollection<Plan> plans  = new Collection<Plan>();
                for (int p= 0;p < cantPlan; p++)
                {
                    string planName = datas.Adjetives[r.Next(0, datas.Adjetives.Count)];

                    Plan plan= new Plan{Id=planId, Name = planName, Price= (float)(r.NextDouble() * DataConstant.MaxPricePlan),
                        Description= datas.Description[r.Next(0, datas.Description.Count)] ,CreatorId=i +1
                    };



                    plan.CreatorId = i+1;


                    plans.Add(plan);

                    planId++;
                }
                creator.Plans = plans;



                
                creadores.Add(creator);

            }
            return creadores;
        }

    }
}
