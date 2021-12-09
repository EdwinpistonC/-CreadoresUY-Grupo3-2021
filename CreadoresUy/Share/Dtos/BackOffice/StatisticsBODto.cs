using System;
using System.Collections.Generic;

namespace Share.Dtos.BackOffice
{
    /*
     * Name y email no se pueden repetir en la base
     */
    public class StatisticsBODto<T,T2>
    {
        public T XValue { get; set; }
        public T2 YValue { get; set; }

    }
}
