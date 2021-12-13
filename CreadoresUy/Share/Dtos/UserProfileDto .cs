using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class UserProfileDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public DateTime Created { get; set; }

        public UserProfileDto(string name, string email, string profileImage, DateTime created)
        {
            Name = name;
            Email = email;
            ProfileImage = profileImage;
            Created = created;
        }

        public UserProfileDto()
        {
        }

        public void FixIfIsNull()
        {
            if(Name == null) Name = "";
            if(Email == null) Email = "";
            if(ProfileImage == null) ProfileImage = "";
        }

    }
}
