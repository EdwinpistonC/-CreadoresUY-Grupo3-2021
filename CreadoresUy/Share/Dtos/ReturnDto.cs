using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class ReturnDto
    {
        public Boolean Succes {  get; set; }
        public ICollection<ItemDto> Items {  get; set; }
        public ReturnDto(Boolean succes, ICollection<ItemDto> items)
        {
            Succes = succes;
            Items = items;
        }
        public ReturnDto()
        {
        }
    }
}
