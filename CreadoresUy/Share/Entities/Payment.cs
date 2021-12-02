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
        public int IdUser { get; set; }
        public int UserPlanId { get; set; }
        public UserPlan UserPlan{ get; set; }
        public DateTime PaymentDate { get; set; }

        public Payment(string externalPaymentId, string nickName)
        {
            ExternalPaymentId = externalPaymentId;
            NickName = nickName;
            PaymentDate = DateTime.Now;
        }
    }
}
