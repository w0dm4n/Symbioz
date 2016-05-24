using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData
{
    public class RawHandlerAttribute : Attribute
    {
        public short RawMessageId { get; set; }
        public RawHandlerAttribute(short rawmessageId)
        {
            this.RawMessageId = rawmessageId;
        }
    }
}
