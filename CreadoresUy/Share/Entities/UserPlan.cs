using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class UserPlan 
    {
        
        public int IdPlan {  get; set; }
        public Plan Plan {  get; set; }
        public int IdUser {  get; set; }
        public User User {  get; set; }

        public DateTime SusbscriptionDate {  get; set; }
        public DateTime ExpirationDate { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public bool Deleted {  get; set; }

        public UserPlan(int idPlan, int idUser, DateTime susbscriptionDate)
        {
            IdPlan = idPlan;
            IdUser = idUser;
            SusbscriptionDate = susbscriptionDate;
            ExpirationDate = DateTime.Now.AddDays(30);
        }

        public UserPlan()
        {
        }
    }
}
