using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class ContentTag
    {

        public int IdContent {  get; set; }
        public Content Content {  get; set; }

        public int IdTag {  get; set; }
        public Tag Tag {  get; set; }


    }
}
