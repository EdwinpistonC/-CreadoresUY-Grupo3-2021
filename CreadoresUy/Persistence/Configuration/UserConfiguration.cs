﻿using Microsoft.EntityFrameworkCore;
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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // SQL config

            builder.ToTable("User");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.IsAdmin).HasDefaultValue(false);


            //Seed Data

            builder.HasData(SeedDatabase());

        }


        public Collection<User> SeedDatabase()
        {
            var creadores = new Collection<User>();

            var datas = new DataConstant();
            Random r = new Random();

            Dictionary<string, string> control =
                new Dictionary<string, string>();




            creadores.Add(
                    new User { Id = 1, Created = DateTime.Now,  Name = "admin", Password = "admin123", Email = "admin@admin" ,IsAdmin=true});
            for (int i = 1; i < DataConstant.UserQuantity; i++)
            {

                string name;
                string surname;
                do
                { 
                    name = datas.Names[r.Next(0, datas.Names.Count)];
                    surname = datas.Names[r.Next(0, datas.Names.Count)];
                } while (control.ContainsKey(name+surname)) ;

                control.Add(name+surname, surname+surname);

                string domain = datas.Domains[r.Next(0, datas.Domains.Count)];

                var email = name + surname +"@"+ domain;

                string password = name + surname;

                if (i < DataConstant.CreatorQuantity)
                {
                    creadores.Add(
                    new User { Id = i + 1,Created= DateTime.Now, CreatorId = i + 1, Name = name+ surname,Password= password, Email = email });
                }
                else
                {
                    creadores.Add(
                        new User { Id = i + 1, Created = DateTime.Now, Name = name + surname, Password = password, Email = email });
                }
                
            }
            return creadores;
        }
    }
}
