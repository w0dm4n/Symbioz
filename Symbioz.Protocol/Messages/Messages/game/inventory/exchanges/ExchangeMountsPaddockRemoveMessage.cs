


















// Generated on 06/04/2015 18:44:53
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeMountsPaddockRemoveMessage : Message
{

public const ushort Id = 6559;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> mountsId;
        

public ExchangeMountsPaddockRemoveMessage()
{
}

public ExchangeMountsPaddockRemoveMessage(IEnumerable<int> mountsId)
        {
            this.mountsId = mountsId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)mountsId.Count());
            foreach (var entry in mountsId)
            {
                 writer.WriteVarInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            mountsId = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (mountsId as int[])[i] = reader.ReadVarInt();
            }
            

}


}


}