using Share.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class MessageDto
    {
        public int IdCreator { get; set; }
        public int IdUser { get; set; }

        public string Text {  get; set; }

        public TipoEmisor TipoEmisor { get; set; }
    }
}
