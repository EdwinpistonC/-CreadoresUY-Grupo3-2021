using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class PagoPlataforma : BaseEntity
    {
        public double Amount { get; set; }
        public DateTime AdeedDate { get; set; }
        public int IdPayment { get; set; }
        public Payment Payment { get; set; }

        public PagoPlataforma(double amount, int idPayment)
        {
            Amount = amount;
            AdeedDate = DateTime.Today;
            IdPayment = idPayment;
        }
    }
}
