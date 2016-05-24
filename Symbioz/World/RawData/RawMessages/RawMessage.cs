using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.RawMessages
{
    public abstract class RawMessage
    {
        public abstract void Serialize(BigEndianWriter writer);
        public abstract void Deserialize(BigEndianReader reader);
    }
}
