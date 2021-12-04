using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class Tag : BaseEntity
    {
        public string Name {  get; set; }

        public ICollection<ContentTag> ContentTags { get; set; }

        public Tag()
        {
            ContentTags = new List<ContentTag>();
        }
        public Tag(string name)
        {
            Name = name;
            ContentTags = new List<ContentTag>();
        }
    }
}
