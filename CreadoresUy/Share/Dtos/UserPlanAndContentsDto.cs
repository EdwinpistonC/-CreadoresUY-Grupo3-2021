using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class UserPlanAndContentsDto
    {
        public bool Follower {  get; set; }
        public int Results {  get; set; }
        public ICollection<ContentAndBoolDto> ContentsAndBool {  get; set; }

        public void FixIsNull()
        {
            Results = Results > 0 ? Results : 0;
            if(ContentsAndBool == null) ContentsAndBool = new List<ContentAndBoolDto>();
        }
        public void OrderByDate()
        {
            ContentsAndBool = ContentsAndBool.OrderByDescending(c => c.Content.AddedDate).ToList();
        }
    }
}
