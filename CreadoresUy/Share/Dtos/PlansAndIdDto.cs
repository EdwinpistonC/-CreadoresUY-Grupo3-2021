using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class PlansAndIdDto
    {
        public int SubscribedTo { get; set; }
        public ICollection<UpdatePlanAndBenefitsDto> Plans {  get; set; }

    }
}
