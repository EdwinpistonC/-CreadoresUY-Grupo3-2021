using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class UserBODto 
    {
        public string Id { get; set; }
        public bool Deleted { get; set; }
        public string Name {  get; set; }
        public string Email {  get; set; }
        public string Password {  get; set; }
        public string? Description {  get; set; }
        public DateTime Created {  get; set; }
        public DateTime? LasLogin{  get; set; }
        public bool? IsAdmin { get; set; }
        public string? ImgProfile { get; set; }
        public int? CreatorId  { get; set; }

    }
}
