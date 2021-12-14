using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class UserChat
    {
        public int idCreator {  get; set; }
        public int idUser {  get; set; }
        public string imgProfile { get; set; }
        public string name { get; set; }

    }
}
