using System;
using System.Collections.Generic;

namespace Share.Entities
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class StatisticsBODto<T>
    {
        public T XValue { get; set; }
        public Double YValue { get; set; }

    }
}
