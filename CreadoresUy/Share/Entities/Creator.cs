using Share.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    /*
     * CreatorName y NickName no se pueden repetir en la base
     */

    public class Creator : BaseEntity
    {
        public string CreatorName {  get; set; }
        public string NickName {  get; set; }
        public string ContentDescription { get; set; }
        public string Biography { get; set; }
        public string YoutubeLink { get; set; }
        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatorCreated {  get; set; }
        public string WelcomeMsg {  get; set; }
        public int Followers {  get; set; }
        public TipoCategory Category1 { get; set; }
        public TipoCategory Category2 { get; set; }



        // Se referencia al usuario para tener la navegacion dentro de EF 
        public User User {  get; set; }
        public ICollection<Plan> Plans {  get; set; }

        public ICollection<Chat> Chats { get; set; }



    }
}
