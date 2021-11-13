using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class CreatorProfileDto
    {
        public string CreatorName { get; set; }
        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public int CantSeguidores { get; set; }
        public int CantSubscriptores { get; set; }
        public ICollection<ContentDto> Contens {  get; set; }

        public void FixIsNull()
        {
            if(CreatorName == null) CreatorName = "";
            if(CreatorImage == null) CreatorImage = "";
            if(CoverImage == null) CoverImage = "";
            if(Contens == null) Contens = new List<ContentDto>();
        }

    }
}
