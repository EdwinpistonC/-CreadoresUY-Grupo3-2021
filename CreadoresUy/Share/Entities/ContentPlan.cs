using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class ContentPlan
    {
        public int IdContent {  get; set; }

        public Content Content {  get; set; }

        public int IdPlan {  get; set; }

        public Plan Plan {  get; set; }

    }
}
