using Share.Enums;
using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class CreatorBODto 
    {
        public int Id {  get; set; }

        public bool Deleted { get; set; }

        public int UserId { get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }
        public string ContentDescription { get; set; }
        public string Biography { get; set; }
        public string YoutubeLink { get; set; }
        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public DateTime CreatorCreated { get; set; }
        public string WelcomeMsg { get; set; }
        public int Followers { get; set; }
        public String Category1 { get; set; }
        public String Category2 { get; set; }
        public void NoNulls()
        {
            if (CreatorName == null) CreatorName = "";
            if (NickName == null) NickName = "";
            if (ContentDescription == null) ContentDescription = "";
            if (Biography == null) Biography = "";
            if (YoutubeLink == null) YoutubeLink = "";
            if (CreatorImage == null) CreatorImage = "";
            if (CoverImage == null) CoverImage = "";
            if (CreatorCreated == null) CreatorCreated = DateTime.MinValue;
            if (WelcomeMsg == null) WelcomeMsg = "";
            if (Followers == null) Followers = 0;
            if (Category1 == null) Category1 = "";
            if (Category2 == null) Category2 ="" ;
        }

    }


}
