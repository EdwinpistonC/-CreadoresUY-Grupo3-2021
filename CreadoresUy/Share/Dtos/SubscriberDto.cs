using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class SubscriberDto
    {
        public string Name {  get; set; }
        public int Id { get; set; }
        public string ProfileImage { get; set; }    
        public DateTime SubscriberDate { get; set; }

        public SubscriberDto(string name, string profileImage, DateTime subscriberDate)
        {
            Name = name;
            ProfileImage = profileImage;
            SubscriberDate = subscriberDate;
        }
    }
}
