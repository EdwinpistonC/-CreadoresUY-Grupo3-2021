using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class CreatorRawDto
    {
        public int UserId { get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }
        public string CreatorDescription { get; set; }
        public string YoutubeLink { get; set; }
        public string WelcomeMsg { get; set; }

        public int Followers { get; set; }

        public TipoCategory Category1 { get; set; }
        public TipoCategory Category2 { get; set; }
    }
}
