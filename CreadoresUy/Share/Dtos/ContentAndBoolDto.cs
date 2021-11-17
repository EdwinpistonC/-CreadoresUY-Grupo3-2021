using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Dtos
{
    public class ContentAndBoolDto
    {
        public bool Authorized { get; set; }
        public ContentDto Content {  get; set; }

        public ContentAndBoolDto(ContentDto content, bool authorized)
        {
            Content = content;
            Authorized = authorized;
        }
    }
}
