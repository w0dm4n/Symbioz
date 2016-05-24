


















// Generated on 06/04/2015 18:45:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObjectsDeletedMessage : Message
{

public const ushort Id = 6034;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<uint> objectUID;
        

public ObjectsDeletedMessage()
{
}

public ObjectsDeletedMessage(IEnumerable<uint> objectUID)
        {
            this.objectUID = objectUID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)objectUID.Count());
            foreach (var entry in objectUID)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            objectUID = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objectUID as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}