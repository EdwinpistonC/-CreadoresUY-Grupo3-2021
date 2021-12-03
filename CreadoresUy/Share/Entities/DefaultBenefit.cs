using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class DefaultBenefit : BaseEntity
    {


        public string Description { get; set; }

        public int IdDefaultPlan { get; set; }
        public DefaultPlan DefaultPlan { get; set; }
    }

}
