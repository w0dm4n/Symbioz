


















// Generated on 06/04/2015 18:45:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartsListMessage : Message
{

public const ushort Id = 1502;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.ContentPart> parts;
        

public PartsListMessage()
{
}

public PartsListMessage(IEnumerable<Types.ContentPart> parts)
        {
            this.parts = parts;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)parts.Count());
            foreach (var entry in parts)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            parts = new Types.ContentPart[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parts as Types.ContentPart[])[i] = new Types.ContentPart();
                 (parts as Types.ContentPart[])[i].Deserialize(reader);
            }
            

}


}


}