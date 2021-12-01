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

        public Plan() { }
        public Plan(string name, string description, float price, string image, string subscriptionMsg, string welcomeVideoLink, int creatorId, Creator creator)
        {
            Name = name;
            Description = description;
            Price = price;
            Image = image;
            SubscriptionMsg = subscriptionMsg;
            WelcomeVideoLink = welcomeVideoLink;
            CreatorId = creatorId;
            Creator = creator;
            Benefits = new List<Benefit>();
            UserPlans = new List<UserPlan>();
            ContentPlans = new List<ContentPlan>();
        }
    }
}
