


















// Generated on 06/04/2015 18:44:52
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
{

public const ushort Id = 6000;
public override ushort MessageId
{
    get { return Id; }
}

public ushort objectGenericId;
        

public ExchangeCraftResultWithObjectIdMessage()
{
}

public ExchangeCraftResultWithObjectIdMessage(sbyte craftResult, ushort objectGenericId)
         : base(craftResult)
        {
            this.objectGenericId = objectGenericId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(objectGenericId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            objectGenericId = reader.ReadVarUhShort();
            if (objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
            

}


}


}