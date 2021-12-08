using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class SubscribeToDto
    {
        public int IdUser {  get; set; }
        public string NickName {  get; set; }
        public int IdPlan { get; set; }
        public string ExternalPaymentId {  get; set; }
        public double PaymentAmount { get; set; }

    }
}
