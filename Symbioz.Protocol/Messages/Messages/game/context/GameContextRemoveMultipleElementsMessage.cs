


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextRemoveMultipleElementsMessage : Message
{

public const ushort Id = 252;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> id;
        

public GameContextRemoveMultipleElementsMessage()
{
}

public GameContextRemoveMultipleElementsMessage(IEnumerable<int> id)
        {
            this.id = id;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)id.Count());
            foreach (var entry in id)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            id = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (id as int[])[i] = reader.ReadInt();
            }
            

}


}


}