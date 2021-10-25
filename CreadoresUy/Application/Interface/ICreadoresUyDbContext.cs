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

        DbSet<UserPlan> UserPlans { get; set; }

        Task<int> SaveChangesAsync();
        public string GenerateJWT(User user);
        public User GetById(int id);

    }
}