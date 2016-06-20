using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.RawMessages
{
    public class MerchantMessage : RawMessage
    {
        public const short Id = 10;

        public string Message { get; set; }

        public MerchantMessage() { }
        public MerchantMessage(string value)
        {
            this.Message = value;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(this.Message);
        }

        public override void Deserialize(BigEndianReader reader)
        {
            this.Message = reader.ReadUTF();
        }
    }
}
