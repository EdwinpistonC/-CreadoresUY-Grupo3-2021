﻿using System;
using System.Collections.Generic;
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
        public string CreatorDescription {  get; set; }
        public DateTime CreatorCreated {  get; set; }
        public string YoutubeLink {  get; set; }
        public string WelcomeMsg {  get; set; }
        public int Followers {  get; set; }

        // Se referencia al usuario para tener la navegacion dentro de EF 
        public User User {  get; set; }
        public ICollection<Plan> Plans {  get; set; }

        public ICollection<Chat> Chats { get; set; }

    }
}
