


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeSetCraftRecipeMessage : Message
{

public const ushort Id = 6389;
public override ushort MessageId
{
    get { return Id; }
}

public ushort objectGID;
        

public ExchangeSetCraftRecipeMessage()
{
}

public ExchangeSetCraftRecipeMessage(ushort objectGID)
        {
            this.objectGID = objectGID;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(objectGID);
            

}

public override void Deserialize(ICustomDataInput reader)
{

objectGID = reader.ReadVarUhShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            

}


}


}