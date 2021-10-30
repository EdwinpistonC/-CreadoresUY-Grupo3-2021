using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public  class AdminDto
    {        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public string? ImgProfile { get; set; }
    }
}
