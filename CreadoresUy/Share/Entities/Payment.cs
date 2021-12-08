using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class Payment : BaseEntity
    {
        public string ExternalPaymentId { get; set; }
        public string NickName {  get; set; }
        public double PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Expired { get; set; }
        //FKs 
        public int UserPlanIdP { get; set; }
        public int UserPlanIdU { get; set; }
        public UserPlan UserPlan{ get; set; }
        public ICollection<PagoCreador> GananciasCreador {  get; set; }
        public ICollection<PagoPlataforma> GananciasPlataforma { get; set; }

        public Payment(string externalPaymentId, string nickName, double paymentAmount, int userPlanIdP, int userPlanIdU )
        {
            ExternalPaymentId = externalPaymentId;
            NickName = nickName;
            PaymentAmount = paymentAmount;
            PaymentDate = DateTime.Now;
            Expired = false;
            UserPlanIdP = userPlanIdP;
            UserPlanIdU = userPlanIdU;
            
        }
    }
}
