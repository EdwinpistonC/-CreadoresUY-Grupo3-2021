using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Configuration;
using Persistence.Constant;
using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Context
{
    public class CreadoresUyDbContext : DbContext, ICreadoresUyDbContext
    {
        static public readonly string KEY= "ThisismySecretKey";
        static public readonly string ISSUER= "Test.com";

        public DbSet<User> Users { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Content> Contents { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<DefaultBenefit> DefaultBenefits { get; set; }
        public DbSet<DefaultPlan> DefaultPlans { get; set; }
        public DbSet<UserCreator> UserCreators { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }
        public DbSet<Content> Category { get; set; }

        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<BanckAccount> BanckAccounts { get; set; }

        public DbSet<FinancialEntity> FinancialEntities {  get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<ContentPlan> ContentPlans { get; set; }

        public DbSet<PagoCreador> PagosCreador { get; set; }

        public DbSet<PagoPlataforma> PagosPlataforma { get; set; }

        public CreadoresUyDbContext()
        {
        }
        public CreadoresUyDbContext(DbContextOptions<CreadoresUyDbContext> options)
           : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {




            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CreatorConfiguration());

            modelBuilder.ApplyConfiguration(new BenefitConfiguration());

            modelBuilder.ApplyConfiguration(new FinancialEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            modelBuilder.Entity<UserPlan>().HasKey(up => new { up.IdUser, up.IdPlan });

            modelBuilder.Entity<UserPlan>()
            .HasOne<User>(up => up.User)
            .WithMany(u => u.UserPlans)
            .HasForeignKey(up => up.IdUser);

            modelBuilder.Entity<UserPlan>()
            .HasOne<Plan>(up => up.Plan)
            .WithMany(p => p.UserPlans)
            .HasForeignKey(up => up.IdPlan);

            modelBuilder.Entity<UserPlan>().Property(up => up.SusbscriptionDate);


            modelBuilder.Entity<ContentTag>().HasKey(ct => new { ct.IdTag, ct.IdContent });

            modelBuilder.Entity<ContentTag>()
            .HasOne<Content>(ct => ct.Content)
            .WithMany(c => c.ContentTags)
            .HasForeignKey(ct => ct.IdContent);

            modelBuilder.Entity<ContentTag>()
            .HasOne<Tag>(ct => ct.Tag)
            .WithMany(t => t.ContentTags)
            .HasForeignKey(ct => ct.IdTag);

            modelBuilder.Entity<ContentPlan>().HasKey(cp => new { cp.IdContent, cp.IdPlan });

            modelBuilder.Entity<ContentPlan>()
            .HasOne<Content>(cp => cp.Content)
            .WithMany(c => c.ContentPlans)
            .HasForeignKey(cp => cp.IdContent);

            modelBuilder.Entity<ContentPlan>()
            .HasOne<Plan>(cp => cp.Plan)
            .WithMany(p => p.ContentPlans)
            .HasForeignKey(cp => cp.IdPlan);

            

            modelBuilder.Entity<Message>()
            .HasOne<Chat>(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.IdChat);


            modelBuilder.Entity<UserCreator>()
            .HasKey(uc => new { uc.IdUser, uc.IdCreator });
            modelBuilder.Entity<UserCreator>()
                .HasOne(uc => uc.User)
                .WithMany(b => b.UserCreators)
                .HasForeignKey(bc => bc.IdUser);
            modelBuilder.Entity<UserCreator>()
                .HasOne(uc => uc.Creator)
                .WithMany(b => b.UserCreators)
                .HasForeignKey(bc => bc.IdCreator);





            modelBuilder.Entity<Message>()
            .HasOne<User>(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.IdUser);


            modelBuilder.Entity<Chat>()
            .HasOne<User>(c => c.User)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.IdUser)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict); ;

            modelBuilder.Entity<Chat>()
            .HasOne<Creator>(c => c.Creator)
            .WithMany(cr => cr.Chats)
            .HasForeignKey(c => c.IdCreator);

            //Relacion para finanzas

            modelBuilder.Entity<Creator>()
            .HasOne(c => c.BanckAccount)
            .WithOne(p => p.Creator)
            .HasForeignKey<BanckAccount>(b => b.CreatorId);

            modelBuilder.Entity<BanckAccount>()
            .HasOne<FinancialEntity>(ba => ba.FinancialEntity)
            .WithMany(f => f.BanckAccounts)
            .HasForeignKey(ba => ba.FinancialEntityId);

            modelBuilder.Entity<DefaultBenefit>()
                .HasOne(db => db.DefaultPlan)
                .WithMany(b => b.Benefits)
                .HasForeignKey(bc => bc.IdDefaultPlan);

            modelBuilder.Entity<Payment>()
            .HasOne<UserPlan>(py => py.UserPlan)
            .WithMany(p => p.Payments)
            .HasForeignKey(py => new { py.UserPlanIdP, py.UserPlanIdU});

            modelBuilder.Entity<PagoCreador>()
                .HasOne(p => p.Payment)
                .WithMany(py => py.GananciasCreador)
                .HasForeignKey(p => p.IdPayment);

            modelBuilder.Entity<PagoPlataforma>()
                .HasOne(p => p.Payment)
                .WithMany(py => py.GananciasPlataforma)
                .HasForeignKey(p => p.IdPayment);


            Seed(modelBuilder);
            Seed1(modelBuilder);
        }
        public string GenerateJWT(User user)
        {

            String signingKey = null;


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = Array.Empty<Claim>();
            var rol = "user";
            if (user.CreatorId != null)
            {
                rol = "creator";
            }

            if (user.IsAdmin == true){
                rol="admin";
            }
            

            claims.Append(new Claim(JwtRegisteredClaimNames.Sub, user.Name));
            claims.Append(new Claim(ClaimTypes.Role, rol));
            claims.Append(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Append(new Claim("Date", DateTime.Now.ToString()));
            claims.Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(ISSUER,
              ISSUER,
              claims,    //null original value
              signingCredentials: credentials);
            Console.WriteLine(token);
            var writeToken = new JwtSecurityTokenHandler().WriteToken(token); //return access token
            Console.WriteLine(writeToken);
            return writeToken;
        }
        public User GetById(int id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }


        public void Seed(ModelBuilder modelBuilder)
        {

            CreatorSeed(modelBuilder);
        }
        public void Seed1(ModelBuilder modelBuilder)
        {
            DefaultPlanSeed(modelBuilder);
        }

        public void CreatorSeed(ModelBuilder modelBuilder)
        {
            var creadores = new Collection<Creator>();
            ICollection<Content> contents = new Collection<Content>();
            ICollection<Tag> tags = new Collection<Tag>();
            ICollection<Plan> plans = new Collection<Plan>();
            ICollection<Benefit> benefits = new Collection<Benefit>();

            ICollection<ContentPlan> contentPlans = new Collection<ContentPlan>();
            ICollection<ContentTag> contentTags = new Collection<ContentTag>();

            var datas = new DataConstant();
            Random r = new Random();

            var planId = 1;
            var contentId = 1;
            var tagId = 1;
            var benefitId = 1;

            for (int i = 0; i < DataConstant.CreatorQuantity; i++)
            {
                var name = datas.Names[r.Next(0, datas.Names.Count)]+i;
                var imagen = DataConstant.ImageProfile[r.Next(0, DataConstant.ImageProfile.Count)];
                Creator creator =
                    new Creator {
                        Id = i + 1,
                        ContentDescription = name,
                        NickName = name,
                        CreatorName = String.Concat(name, "Creator"),
                        Category1 = "Arte",
                        Category2 = "Deporte",
                        CreatorImage = imagen,
                        YoutubeLink = DataConstant.VideoPresentacion,
                        CoverImage = DataConstant.Banner,
                        Biography = "Hola soy " + name + " pero me conocen por " + name,
                        Followers = r.Next(0, 1000)
                    };

                int cantPlan = r.Next(3, r.Next(3, DataConstant.MaxPlans));

                for (int p = 0; p < cantPlan; p++)
                {
                    string planName = datas.Adjetives[r.Next(0, datas.Adjetives.Count)];

                    Plan plan = new Plan
                    {
                        Id = planId,
                        Name = planName,
                        Price = (float)(r.NextDouble() * DataConstant.MaxPricePlan),
                        Description = datas.Description[r.Next(0, datas.Description.Count)],
                        CreatorId = i + 1
                    };

                    int cantBenefits = r.Next(3, r.Next(3, DataConstant.MaxBenefits));


                    plan.CreatorId = i + 1;
                    plans.Add(plan);


                    for (int b = 0; b < cantBenefits; b++)
                    {
                        Benefit benefit = new Benefit
                        {
                            Id = benefitId,
                            Description= datas.Benefits[r.Next(0,datas.Benefits.Count)],
                            IdPlan = planId
                        };

                        benefits.Add(benefit);
                        benefitId++;
                    }
                    planId++;

                }
                int cantContent = r.Next(3, r.Next(3, DataConstant.MaxPlans));

              
                for (int c = 0; c < cantContent; c++)
                {
                    int contentSelected = r.Next(0, datas.ContentTiles.Count);

                    Content content = new Content
                    {
                        Id = contentId,
                        Title = datas.ContentTiles[contentSelected],
                        Description = datas.ContentTexts[contentSelected],
                        AddedDate=DateTime.Now,
                        Type=TipoContent.Text
                    };


                    var newIdPlan = r.Next(planId - cantPlan - 1, planId - 1);

                    if (newIdPlan <= 0) newIdPlan = 1;

                    ContentPlan contentPlan = new ContentPlan { 
                         IdContent= contentId,
                         IdPlan= newIdPlan
                    };
                    int cantTags = r.Next(1, r.Next(1, DataConstant.MaxTags));
                    

                    for (int t = 0; t < cantTags; t++)
                    {
                        int tagSelected = r.Next(0, datas.Tags.Count);

                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = datas.Tags[tagSelected],
                            
                        };
                        ContentTag contentTag = new ContentTag
                        {
                             IdContent= contentId,
                             IdTag=tagId
                        };

                        tags.Add(tag);
                        contentTags.Add(contentTag);
                        tagId++;
                    }



                    contents.Add(content);
                    contentPlans.Add(contentPlan);
                    contentId++;
                }
               

                creadores.Add(creator);
            }



            modelBuilder.Entity<Creator>().HasData(creadores);
            modelBuilder.Entity<Content>().HasData(contents);

            modelBuilder.Entity<Plan>().HasData(plans);

            modelBuilder.Entity<Tag>().HasData(tags);
            modelBuilder.Entity<Benefit>().HasData(benefits);

            modelBuilder.Entity<ContentPlan>().HasData(contentPlans);
            modelBuilder.Entity<ContentTag>().HasData(contentTags);
        }

        public void DefaultPlanSeed(ModelBuilder modelBuilder)
        {
            var planes = new Collection<DefaultPlan>();
            ICollection<DefaultBenefit> beneficios = new Collection<DefaultBenefit>();

            var plan1 = new DefaultPlan
            {
                Id = 1,
                Name = "Basico",
                Description = "Plan Basico Free",
                Price = 0,
                Image = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/Planes%2Fplan%20b.png?alt=media&token=e843ca33-19d8-4cd4-ba4d-342d9c798572",
                SubscriptionMsg = "Bienvenidos a tod@s",
                WelcomeVideoLink = "tuVideo.com"
            };

            var plan2 = new DefaultPlan
            {
                Id = 2,
                Name = "Estandar",
                Description = "Plan Estandar ",
                Price = 150,
                Image = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/Planes%2Fplan%20e.png?alt=media&token=1525fdd8-8bcc-4666-b000-65de46fbdda9",
                SubscriptionMsg = "Bienvenidos a tod@s",
                WelcomeVideoLink = "tuVideo.com"
            };

            var plan3 = new DefaultPlan
            {
                Id = 3,
                Name = "Premium",
                Description = "Plan Premium",
                Price = 350,
                Image = "https://firebasestorage.googleapis.com/v0/b/creadoresuy-674c1.appspot.com/o/Planes%2Fplan%20p.jfif?alt=media&token=a5d3cfa1-864e-4e4a-9831-caa1b2ea83a6",
                SubscriptionMsg = "Bienvenidos a tod@s",
                WelcomeVideoLink = "tuVideo.com"
            };
            planes.Add(plan1); planes.Add(plan2); planes.Add(plan3);

            var dfb1 = new DefaultBenefit
            {
                Id = 1,
                Description = "Contenidos Libres",
                IdDefaultPlan = plan1.Id,
            };
            var dfb2 = new DefaultBenefit
            {
                Id = 2,
                Description = "Todas los meses un contenido libre ",
                IdDefaultPlan = plan1.Id,
            };


            var dfb3 = new DefaultBenefit
            {
                Id = 3,
                Description = "Plan Basico Free + Contenidos Exclusivos",
                IdDefaultPlan = plan2.Id,
            };
            var dfb4 = new DefaultBenefit
            {
                Id = 4,
                Description = "Todas las semanas contenidos nuevos",
                IdDefaultPlan = plan2.Id,
            };


            var dfb5 = new DefaultBenefit
            {
                Id = 5,
                Description = "Todos los dias contenidos nuevos",
                IdDefaultPlan = plan3.Id,
            };
            var dfb6 = new DefaultBenefit
            {
                Id = 6,
                Description = "Plan Estandar + Contenidos Exclusivos",
                IdDefaultPlan = plan3.Id,
            };
            var dfb7 = new DefaultBenefit
            {
                Id = 7,
                Description = "Chat",
                IdDefaultPlan = plan3.Id,
            };

            beneficios.Add(dfb1); beneficios.Add(dfb2); beneficios.Add(dfb3);
            beneficios.Add(dfb4); beneficios.Add(dfb5); beneficios.Add(dfb6);
            beneficios.Add(dfb7);
            modelBuilder.Entity<DefaultPlan>().HasData(planes);
            modelBuilder.Entity<DefaultBenefit>().HasData(beneficios);

        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
public interface IJwtAuth
{
    string Authentication(string username, string password);
}

public class Auth : IJwtAuth
{
    private readonly string username = "kirtesh";
    private readonly string password = "Demo1";
    private readonly string key;
    public Auth(string key)
    {
        this.key = key;
    }
    public string Authentication(string username, string password)
    {
        if (!(username.Equals(username) || password.Equals(password)))
        {
            return null;
        }

        // 1. Create Security Token Handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // 2. Create Private Key to Encrypted
        var tokenKey = Encoding.ASCII.GetBytes(key);

        //3. Create JETdescriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                        new Claim(ClaimTypes.Name, username)
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        //4. Create Token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // 5. Return Token from method
        return tokenHandler.WriteToken(token);
    }
}