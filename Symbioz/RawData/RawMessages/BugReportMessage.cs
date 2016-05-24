using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.RawMessages
{
    public class BugReportMessage : RawMessage
    {
        public const short Id = 1;
        
        public string Value { get; set; }

        public BugReportMessage() { }
        public BugReportMessage(string value)
        {
            this.Value = value;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(Value);
        }

        public override void Deserialize(BigEndianReader reader)
        {
            this.Value = reader.ReadUTF();
        }
    }
}
