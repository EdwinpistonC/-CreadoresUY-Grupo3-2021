using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class DefaultPlan : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string SubscriptionMsg { get; set; }
        public string WelcomeVideoLink { get; set; }
        public ICollection<DefaultBenefit> Benefits { get; set; }
    }
}
