using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class InfoPlanDto
    {
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }


        public void NoNulls()
        {
            if(SubscriptionMsg == null) SubscriptionMsg = string.Empty;
            if (WelcomeVideoLink == null) WelcomeVideoLink = string.Empty;
        }
    }
}
