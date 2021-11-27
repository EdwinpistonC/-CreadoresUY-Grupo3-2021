using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class FinancialEntity :BaseEntity
    {
        public int RUT {  get; set; }    
        public string Name {  get; set; }
        public string Country {  get; set; }
        public int Phone {  get; set; }
        public ICollection<BanckAccount> BanckAccounts { get; set; }

        public FinancialEntity(int Rut, string name, string country, int phone)
        {
            RUT = Rut;
            Name = name;
            Country = country;
            Phone = phone;
            BanckAccounts = new List<BanckAccount>();
        }

        public FinancialEntity()
        {
        }
    }
}
