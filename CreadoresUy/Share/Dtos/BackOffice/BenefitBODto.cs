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
        public string Description { get; set; }
        public bool? Deleted { get; set; }

        public void NoNulls()
        {
            if (Description == null) Description = "";
            if (Id == null) Id = 0;
        }

    }


}
