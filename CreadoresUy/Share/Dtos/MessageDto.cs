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
        public int IdSender { get; set; }
        public int IdReceiver { get; set; }

        public string Text {  get; set; }

        public TipoEmisor TipoEmisor { get; set; }
    }
}
