


















// Generated on 06/04/2015 18:44:49
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StatedMapUpdateMessage : Message
{

public const ushort Id = 5716;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.StatedElement> statedElements;
        

public StatedMapUpdateMessage()
{
}

public StatedMapUpdateMessage(IEnumerable<Types.StatedElement> statedElements)
        {
            this.statedElements = statedElements;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)statedElements.Count());
            foreach (var entry in statedElements)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            statedElements = new Types.StatedElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (statedElements as Types.StatedElement[])[i] = new Types.StatedElement();
                 (statedElements as Types.StatedElement[])[i].Deserialize(reader);
            }
            

}


}


}