using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class PlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }
        public int CreatorId { get; set; }
        public ICollection<BenefitDTO> Benefits { get; set; }
        public void FixIfIsNull()
        {
            if (Name == null) Name = "";
            if (Description == null) Description = "";
            if (Price == null) Price = 0;
            if (Image == null) Image = "";
            if (SubscriptionMsg == null) SubscriptionMsg = "";
            if (WelcomeVideoLink == null) WelcomeVideoLink = "";
            if (CreatorId == null) CreatorId = 0;
            if (Benefits == null) Benefits = new List<BenefitDTO>();
        }


    }
}
