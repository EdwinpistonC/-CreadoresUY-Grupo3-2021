using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class UpdatePlanAndBenefitsDto
    {
        public int IdPlan {  get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }
        public ICollection<string> Benefits { get; set; }

        public void FixIsNull()
        {
            if (Name == null) Name = string.Empty;
            if (Description == null) Description = string.Empty;
            if (Image == null) Image = string.Empty;
            if (SubscriptionMsg == null) SubscriptionMsg = string.Empty;
            if (WelcomeVideoLink == null) WelcomeVideoLink = string.Empty;
            if(Benefits == null) Benefits = new List<string>(); 
        }
    }
}
