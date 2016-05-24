using Symbioz.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.RawData.RawMessages
{
    public class FilesDirMessage : RawMessage
    {
        public const short Id = 3;

        public string[] Files { get; set; }

        public FilesDirMessage(string[] files)
        {
            this.Files = files;
        }
        public FilesDirMessage()
        {

        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(Files.Length);
            foreach (var file in Files)
            {
                writer.WriteUTF(file);
            }
        }

        public override void Deserialize(BigEndianReader reader)
        {
            int num = reader.ReadInt();
            Files = new string[num];
            for (int i = 0; i < num; i++)
            {
                Files[i] = reader.ReadUTF();
            }
        }
    }
}
