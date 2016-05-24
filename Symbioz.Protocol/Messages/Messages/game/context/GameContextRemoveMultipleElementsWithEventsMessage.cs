


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextRemoveMultipleElementsWithEventsMessage : GameContextRemoveMultipleElementsMessage
{

public const ushort Id = 6416;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<sbyte> elementEventIds;
        

public GameContextRemoveMultipleElementsWithEventsMessage()
{
}

public GameContextRemoveMultipleElementsWithEventsMessage(IEnumerable<int> id, IEnumerable<sbyte> elementEventIds)
         : base(id)
        {
            this.elementEventIds = elementEventIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)elementEventIds.Count());
            foreach (var entry in elementEventIds)
            {
                 writer.WriteSByte(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            elementEventIds = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (elementEventIds as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}