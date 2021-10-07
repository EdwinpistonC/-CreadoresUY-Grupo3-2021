using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class Message : BaseEntity
    {
        public int IdUser {  get; set; }
        public User User {  get; set; }

        public string Text;
        public DateTime Sended {  get; set; }

        public int IdChat {  get; set; }
        public Chat Chat {  get; set; }


    }
}
