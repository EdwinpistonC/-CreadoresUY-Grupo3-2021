using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class CreadorDatabaseDto
    {

        public int Id { get; set; }
        public bool Deleted { get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }
        public string CreatorDescription { get; set; }
        public DateTime CreatorCreated { get; set; }
        public string YoutubeLink { get; set; }
        public string WelcomeMsg { get; set; }
        public int Followers { get; set; }

        public TipoCategory Category1 { get; set; }
        public TipoCategory Category2 { get; set; }

        public void FixIfIsNull()
        {
            if (CreatorName == null) CreatorName = "";
            if (NickName == null) NickName = "";
            if (CreatorDescription == null) CreatorDescription = "";
            if (CreatorCreated == null) CreatorCreated = DateTime.Now;
            if (YoutubeLink == null) YoutubeLink = "";
            if (WelcomeMsg == null) WelcomeMsg = "";
            if (Followers == null) Followers = 0;
            if (Category1 == null) Category1 = 0;
            if (Category2 == null) Category2 = 0;


        }
    }
}
