


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AcquaintanceServerListMessage : Message
{

public const ushort Id = 6142;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<ushort> servers;
        

public AcquaintanceServerListMessage()
{
}

public AcquaintanceServerListMessage(IEnumerable<ushort> servers)
        {
            this.servers = servers;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)servers.Count());
            foreach (var entry in servers)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            servers = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (servers as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}