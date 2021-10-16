using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public  class Plan : BaseEntity
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public float Price {  get; set; }
        public string Image {  get; set; }
        public string SubscriptionMsg {  get; set; }
        public string WelcomeVideoLink {  get; set; }

        public int CreatorId {  get; set; }
        public Creator Creator {  get; set; }
        public ICollection<Benefit> Benefits { get; set; }
        public ICollection<UserPlan> UserPlans { get; set; }
        public ICollection<ContentPlan> ContentPlans { get; set; }


    }
}
