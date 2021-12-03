using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class DefaultPlanBODto 
    {
        public int? Id { get; set; }
        public bool? Deleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }

        public ICollection<BenefitBODto> Benefits { get; set; }


        public void NoNulls()
        {
            if (Id == null) Id = 0;
            if (Name == null) Name = "";
            if (Deleted == null) Deleted = false;
            if (Description == null) Description = "";
            if (Price == null) Price = 0;
            if (Image == null) Image = "";
            if (SubscriptionMsg == null) SubscriptionMsg = "";
            if (WelcomeVideoLink == null) WelcomeVideoLink = "";
            if (Benefits == null) Benefits = new HashSet<BenefitBODto>();
        }

    }


}
