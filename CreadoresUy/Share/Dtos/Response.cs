using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class Response<T>
    {
        public T Obj {  get; set; }
        public Boolean Success {  get; set; }
        public HttpStatusCode CodStatus { get; set; }
        public List<string> Message { get; set; }

    }
}
