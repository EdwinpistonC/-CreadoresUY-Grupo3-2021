using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class CategoryBODto 
    {
        public int? Id { get; set; }
        public bool? Deleted { get; set; }
        public string Name { get; set; }


        public void NoNulls()
        {
            if (Name == null) Name = "";
            if (Deleted == null) Deleted = false;
            if (Id == null) Id = 0;
        }

    }


}
