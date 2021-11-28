using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class UserCreator
    {
        public int IdCreator { get; set; }
        public Creator Creator { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public DateTime DateFollow { get; set; }
        public bool Unfollow { get; set; }
        public DateTime DateUnfollow { get; set; }
        public UserCreator(int idCreator, int idUser, DateTime dateFollow)
        {
            IdCreator = idCreator;
            IdUser = idUser;
            DateFollow = dateFollow;
        }

    }
}
