using Share.Enums;
using System;
using System.Collections.Generic;

namespace Share.Dtos
{
    public class ContentDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime AddedDate { get; set; }
        public bool Draft { get; set; }
        public DateTime DatePublish { get; set; }
        public bool Public { get; set; }
        public string Compositor { get; set; }
        public string Link { get; set; }
        public string Img { get; set; }
        public TipoContent Type { get; set; }

        public ICollection<int> Plans { get; set; }
        public ICollection<TagDto> Tags { get; set; }

    }
}
