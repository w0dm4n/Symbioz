


















// Generated on 06/04/2015 18:44:02
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HelloConnectMessage : Message
{

public const ushort Id = 3;
public override ushort MessageId
{
    get { return Id; }
}

public string salt;
        public IEnumerable<sbyte> key;
        

public HelloConnectMessage()
{
}

public HelloConnectMessage(string salt, IEnumerable<sbyte> key)
        {
            this.salt = salt;
            this.key = key;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(salt);
            writer.WriteUShort((ushort)key.Count());
            foreach (var entry in key)
            {
                 writer.WriteSByte(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

salt = reader.ReadUTF();
            var limit = reader.ReadUShort();
            key = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (key as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}