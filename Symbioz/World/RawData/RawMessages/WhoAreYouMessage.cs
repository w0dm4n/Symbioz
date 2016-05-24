using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.RawMessages
{
    public class WhoAreYouMessage : RawMessage
    {
        public const short Id = 2;

        public string OS { get; set; }
        public string Username { get; set; }

        public WhoAreYouMessage() { }
        public WhoAreYouMessage(string os,string username)
        {
            this.OS = os;
            this.Username = username;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUTF(OS);
            writer.WriteUTF(Username);
        }

        public override void Deserialize(BigEndianReader reader)
        {
            this.OS = reader.ReadUTF();
            this.Username = reader.ReadUTF();
        }
    }
}
