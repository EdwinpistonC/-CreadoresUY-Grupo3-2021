using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class CreatorProfileDto
    {
        public string CreatorName { get; set; }
        public string CreatorImage { get; set; }
        public string CoverImage { get; set; }
        public int CantSeguidores { get; set; }
        public int CantSubscriptores { get; set; }
        public string ContentDescription { get; set; }
        public string Biography { get; set; }
        public string YoutubeLink { get; set; }
        public ICollection<PlanDto> Plans { get; set; }

        public void FixIfIsNull()
        {
            if(CreatorName == null) CreatorName = "";
            if(CreatorImage == null) CreatorImage = "";
            if(CoverImage == null) CoverImage = "";
            if (ContentDescription == null) ContentDescription = "";
            if (Biography == null) Biography = "";
            if (YoutubeLink == null) YoutubeLink = "";
            if (Plans != null )
            {
                if (Plans.Count!=0)
                {
                    List<PlanDto> newPlans = new List<PlanDto>();
                    foreach (PlanDto plan in Plans)
                    {
                        plan.FixIfIsNull();
                        newPlans.Add(plan);
                    }
                    Plans = newPlans;
                }

            }
            else
            {
                Plans = new List<PlanDto>();
            }
        }

    }
}
