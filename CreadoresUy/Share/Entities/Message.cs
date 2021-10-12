using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class Message : BaseEntity
    {
        public Message(int idUser, TipoEmisor tipoEmisor, string text, DateTime sended, int idChat)
        {
            IdUser = idUser;
            TipoEmisor = tipoEmisor;
            Text = text;
            Sended = sended;
            IdChat = idChat;
        }

        public int IdUser { get; set; }
        public User User { get; set; }
        public TipoEmisor TipoEmisor {  get; set; } 
        public string Text {  get; set; }
        public DateTime Sended {  get; set; }

        public int IdChat {  get; set; }
        public Chat Chat {  get; set; }


    }
}
