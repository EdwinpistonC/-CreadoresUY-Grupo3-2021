using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class CreadorSearchDto
    {
        public string Id { get; set; }
        public string CreatorName { get; set; }
        public string NickName { get; set; }

        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public int CantSeguidores { get; set; }
        public int CantSubscriptores { get; set; }
        public string ContentDescription { get; set; }
        public List<String> Categorys { get; set; }

        public void FixIfIsNull()
        {
           if (CreatorName == null) CreatorName = "";
           if (CreatorImage == null) CreatorImage = "";
           if (CoverImage == null) CoverImage = "";
           if (ContentDescription == null) ContentDescription = "";
            if (CantSeguidores == null) CantSeguidores = 0;
            if (CantSubscriptores == null) CantSubscriptores = 0;

        }



    }
}
