using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class PaymentBODto
    {
        public int? Id { get; set; }
        public bool? Nickname { get; set; }
        public string UserName { get; set; }
        public string Plan { get; set; }
        public float Price { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }

    }


}
