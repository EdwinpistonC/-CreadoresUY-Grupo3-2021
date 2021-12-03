using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class BenefitBODto 
    {
        public int? Id { get; set; }
        public string Name { get; set; }


        public void NoNulls()
        {
            if (Name == null) Name = "";
            if (Id == null) Id = 0;
        }

    }


}
