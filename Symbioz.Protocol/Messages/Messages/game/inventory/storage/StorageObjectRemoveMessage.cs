


















// Generated on 06/04/2015 18:45:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StorageObjectRemoveMessage : Message
{

public const ushort Id = 5648;
public override ushort MessageId
{
    get { return Id; }
}

public uint objectUID;
        

public StorageObjectRemoveMessage()
{
}

public StorageObjectRemoveMessage(uint objectUID)
        {
            this.objectUID = objectUID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(objectUID);
            

}

public override void Deserialize(ICustomDataInput reader)
{

objectUID = reader.ReadVarUhInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            

}


}


}