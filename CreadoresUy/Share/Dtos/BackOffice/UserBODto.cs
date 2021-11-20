using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class UserBODto 
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string Name {  get; set; }
        public string Email {  get; set; }
        public string Password {  get; set; }
        public string? Description {  get; set; }
        public DateTime Created {  get; set; }
        public DateTime? LasLogin{  get; set; }
        public string? ImgProfile { get; set; }
        public int? CreatorId  { get; set; }

        public void NoNulls()
        {
            if (Name == null) Name = "";
            if (Email == null) Email = "";
            if (Description == null) Description = "";
            if (ImgProfile == null) ImgProfile ="";
            if (Created == null) Created = DateTime.MinValue;
            if (LasLogin == null) LasLogin = DateTime.MinValue;
            if (CreatorId <=0 || CreatorId == null) CreatorId = 0;
            Password = "";
        }

    }


}
