using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class BenefitCantDto
    {
        public string BenefitName { get; set; }
        public int Cant { get; set; }

        public BenefitCantDto(string benefitName, int cant)
        {
            BenefitName = benefitName;
            Cant = cant;
        }
    }
}
