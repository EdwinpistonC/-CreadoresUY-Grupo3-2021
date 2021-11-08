using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class ContentMinDto
    {
        public int IdCreator { get; set; }
        public string Nickname { get; set; }
        public DateTime PublishDate { get; set; }
        public string image {  get; set; }
        
        public string description {  get; set; }


    }
}
