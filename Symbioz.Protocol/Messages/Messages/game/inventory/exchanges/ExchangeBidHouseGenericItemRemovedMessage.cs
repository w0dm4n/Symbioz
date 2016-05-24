


















// Generated on 06/04/2015 18:44:50
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeBidHouseGenericItemRemovedMessage : Message
{

public const ushort Id = 5948;
public override ushort MessageId
{
    get { return Id; }
}

public ushort objGenericId;
        

public ExchangeBidHouseGenericItemRemovedMessage()
{
}

public ExchangeBidHouseGenericItemRemovedMessage(ushort objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(objGenericId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

objGenericId = reader.ReadVarUhShort();
            if (objGenericId < 0)
                throw new Exception("Forbidden value on objGenericId = " + objGenericId + ", it doesn't respect the following condition : objGenericId < 0");
            

}


}


}