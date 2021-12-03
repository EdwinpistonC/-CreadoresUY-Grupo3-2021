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
            var usuarios = new Collection<User>();

            var datas = new DataConstant();
            Random r = new Random();

            Dictionary<string, string> control =
                new Dictionary<string, string>();





            int i = 0;
            var imagen ="";

            for (i = 0; i < DataConstant.UserQuantity; i++)
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

                imagen = DataConstant.ImageProfile[r.Next(0, DataConstant.ImageProfile.Count)];
                if (i < DataConstant.CreatorQuantity-1)
                {
                    usuarios.Add(
                    new User { Id = i + 1,Created= DateTime.Now, CreatorId = i + 1, Name = name+ surname,Password= password, Email = email,ImgProfile=imagen });
                }
                else
                {
                    usuarios.Add(
                        new User { Id = i + 1, Created = DateTime.Now, Name = name + surname, Password = password, Email = email, ImgProfile = imagen });
                }
           
            }
            imagen = DataConstant.ImageProfile[r.Next(0, DataConstant.ImageProfile.Count)];

            usuarios.Add(
             new User { Id = i+1, Created = DateTime.Now, Name = "admin", Password = "admin123", Email = "admin@admin", IsAdmin = true, ImgProfile = imagen });
            imagen = DataConstant.ImageProfile[r.Next(0, DataConstant.ImageProfile.Count)];
            usuarios.Add(
                    new User { Id = i+2, Created = DateTime.Now, Name = "usuario", Password = "usuario123", Email = "usuario@usuario", ImgProfile = imagen });
            imagen = DataConstant.ImageProfile[r.Next(0, DataConstant.ImageProfile.Count)];
            usuarios.Add(
                    new User { Id = i + 3, Created = DateTime.Now, CreatorId = DataConstant.CreatorQuantity - 1, Name = "creador", Password = "creador123", Email = "creador@creador", ImgProfile = imagen });

            return usuarios;
        }
    }
}
