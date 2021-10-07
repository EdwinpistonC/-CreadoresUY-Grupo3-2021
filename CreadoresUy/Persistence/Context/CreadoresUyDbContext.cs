using Application.Interface;
using Microsoft.EntityFrameworkCore;
using Share;
using Share.Entities;
using System;
using System.Collections.Generic;
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

            modelBuilder.Entity<Creator>()
            .HasOne<User>(c => c.User)
            .WithOne(u => u.Creator)
            .HasForeignKey<User>(u => u.CreatorId);

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

            modelBuilder.Entity<Benefit>()
            .HasOne<Plan>(b => b.Plan)
            .WithMany(p => p.Benefits)
            .HasForeignKey(b=> b.IdPlan);

            modelBuilder.Entity<Message>()
            .HasOne<Chat>(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.IdChat);


            modelBuilder.Entity<Chat>()
            .HasOne<User>(c => c.User)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.IdUser);

            modelBuilder.Entity<Chat>()
            .HasOne<Creator>(c => c.Creator)
            .WithMany(cr => cr.Chats)
            .HasForeignKey(c => c.IdCreator);

        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
