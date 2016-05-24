


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InteractiveMapUpdateMessage : Message
{

public const ushort Id = 5002;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.InteractiveElement> interactiveElements;
        

public InteractiveMapUpdateMessage()
{
}

public InteractiveMapUpdateMessage(IEnumerable<Types.InteractiveElement> interactiveElements)
        {
            this.interactiveElements = interactiveElements;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)interactiveElements.Count());
            foreach (var entry in interactiveElements)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            interactiveElements = new Types.InteractiveElement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (interactiveElements as Types.InteractiveElement[])[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElement>(reader.ReadShort());
                 (interactiveElements as Types.InteractiveElement[])[i].Deserialize(reader);
            }
            

}


}


}