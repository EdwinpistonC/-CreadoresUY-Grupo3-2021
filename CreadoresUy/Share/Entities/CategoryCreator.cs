using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class CategoryCreator
    {
        public int IdCreator { get; set; }

        public Creator Creator { get; set; }

        public int IdCategory { get; set; }

        public Category Category { get; set; }

    }
}
