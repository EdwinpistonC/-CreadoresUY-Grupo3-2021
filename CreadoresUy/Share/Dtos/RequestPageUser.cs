using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class RequestPageUser
    {
        public int PageNumber {  get; set; }
        public int PageSize { get; set; } 

        public RequestPageUser()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public RequestPageUser(int number, int size)
        {
            this.PageNumber = number > 1 ? number : 1;
            this.PageSize = size > 10 ? size : 10 ;
        }

    }
}
