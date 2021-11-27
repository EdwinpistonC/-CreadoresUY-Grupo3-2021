using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class UserCreator
    {

        public UserCreator(int idCreator, int idUser, DateTime dateTime)
        {
            IdCreator = idCreator;
            IdUser = idUser;
            DateTime = dateTime;
        }

        public int IdCreator { get; set; }
        public Creator Creator { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
        public DateTime DateTime { get; set; }



    }
}
