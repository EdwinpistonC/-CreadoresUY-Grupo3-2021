using Microsoft.EntityFrameworkCore;
using Share;
using Share.Dtos;
using Share.Entities;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ICreadoresUyDbContext
    {


        DbSet<User> Users { get; set; }
        DbSet<Creator> Creators { get; set; }
        DbSet<Content> Contents { get; set; }

        DbSet<Chat> Chats { get; set; }

        DbSet<Message> Messages { get; set; }

        DbSet<Plan> Plans { get; set; }

        DbSet<Tag> Tags { get; set; }
        DbSet<DefaultBenefit> DefaultBenefits { get; set; }
        DbSet<DefaultPlan> DefaultPlans { get; set; }
        DbSet<Category> Categorys { get; set; }
        DbSet<UserPlan> UserPlans { get; set; }
        public DbSet<BanckAccount> BanckAccounts { get; set; }

        public DbSet<FinancialEntity> FinancialEntities { get; set; }
        public DbSet<UserCreator> UserCreators { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        Task<int> SaveChangesAsync();
        public string GenerateJWT(User user);
        public User GetById(int id);
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<ContentPlan> ContentPlans { get; set; }
        public DbSet<PagoCreador> PagosCreador { get; set; }
        public DbSet<PagoPlataforma> PagosPlataforma { get; set; }
    }
}