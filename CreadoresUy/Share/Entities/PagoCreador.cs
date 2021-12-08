using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class PagoCreador : BaseEntity
    {
        public double Amount {  get; set; }
        public int IdCreator {  get; set; }
        public string Nickname { get; set; }
        public string PlanName {  get; set; }
        public bool Pending { get; set; }   
        public DateTime AdeedDate {  get; set; } //Dia que se crea el pago 
        public DateTime PayDate { get; set; } // Dia que la plataforma realiza el pago
        public int IdPayment {  get; set; }
        public Payment Payment { get; set; }

        public PagoCreador(double amount, int idCreator, string nickname, string planName, int idPayment)
        {
            Amount = amount;
            IdCreator = idCreator;
            Nickname = nickname;
            PlanName = planName;
            Pending = true;
            AdeedDate = DateTime.Today;
            IdPayment = idPayment;
        }
    }
}
