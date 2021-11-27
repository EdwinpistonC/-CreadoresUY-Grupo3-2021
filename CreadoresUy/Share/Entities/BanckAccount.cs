using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Entities
{
    public class BanckAccount : BaseEntity
    {
        public long AccountNumber { get; set; }
        public string AccountHolder {  get; set; }
        public int CreatorId { get; set; }
        public Creator Creator { get; set; }
        public int FinancialEntityId {  get; set; }
        public FinancialEntity FinancialEntity {  get; set;}
        public DateTime Date {  get; set; }
    }
}
