using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Persistence.Configuration;
using Persistence.Constant;
using Share;
using Share.Entities;
using Share.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class CreadoresUyDbContext : DbContext, ICreadoresUyDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<Content> Contents { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserPlan> UserPlans { get; set; }

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



            modelBuilder.Entity<UserPlan>().HasKey(up => new { up.IdUser, up.IdPlan });

            modelBuilder.Entity<UserPlan>()
            .HasOne<User>(up => up.User)
            .WithMany(u => u.UserPlans)
            .HasForeignKey(up => up.IdUser);

            modelBuilder.Entity<UserPlan>()
            .HasOne<Plan>(up => up.Plan)
            .WithMany(p => p.UserPlans)
            .HasForeignKey(up => up.IdPlan);


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

            Seed(modelBuilder);
        }

        public void Seed(ModelBuilder modelBuilder)
        {

            CreatorSeed(modelBuilder);
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
                var name = datas.Names[r.Next(0, datas.Names.Count)];
                Creator creator = new Creator { Id = i + 1, CreatorDescription = name, NickName = name, CreatorName = String.Concat(name, "Creator") };

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
                        Type=TpoContent.Text
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

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
