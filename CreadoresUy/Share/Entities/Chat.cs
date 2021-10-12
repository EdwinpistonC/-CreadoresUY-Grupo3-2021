using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class Chat : BaseEntity
    {
        public Chat(int idCreator, int idUser)
        {
            IdCreator = idCreator;
            IdUser = idUser;
        }

        public int IdCreator { get; set; }
        public Creator Creator {  get; set; }
        public int IdUser {  get; set; }
        public User User {  get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
