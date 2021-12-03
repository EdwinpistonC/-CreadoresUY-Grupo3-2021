using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class DefaultBenefit : BaseEntity
    {

        public string Description;
        public int IdPlan { get; set; }
        public DefaultPlan Plan { get; set; }
    }
}
