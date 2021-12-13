using System;
using System.Collections.Generic;

namespace Share.Dtos.BackOffice
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class PaymentBODto
    {
        public int IdCreator { get; set; }
        public string Nickname { get; set; }
        public double Amount { get; set; }
        public string Month { get; set; }

    }


}
