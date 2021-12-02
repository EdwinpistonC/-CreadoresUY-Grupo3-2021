using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class UserPlan 
    {
        public UserPlan(int idPlan, int idUser, DateTime dateTime)
        {
            IdPlan = idPlan;
            IdUser = idUser;
            DateTime = dateTime;
            Deleted = false;
        }

        public int IdPlan {  get; set; }
        public Plan Plan {  get; set; }
        public int IdUser {  get; set; }
        public User User {  get; set; }

        public DateTime DateTime {  get; set; }
        public ICollection<Payment> Payments { get; set; }
        public bool Deleted {  get; set; }
    }
}
