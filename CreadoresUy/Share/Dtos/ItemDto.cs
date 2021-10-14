using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class ItemDto
    {
        public int CodStatus {  get; set; }
        public string ResVariable {  get; set; }

        public ItemDto(int codStatus, string resVariable)
        {
            CodStatus = codStatus;
            ResVariable = resVariable;
        }
    }
}
