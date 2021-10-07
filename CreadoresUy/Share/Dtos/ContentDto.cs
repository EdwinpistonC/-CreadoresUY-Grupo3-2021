using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class ContentDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Draft { get; set; }
        public DateTime DatePublish { get; set; }
        public bool Public { get; set; }
        public string Compositor { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }
    }
}
