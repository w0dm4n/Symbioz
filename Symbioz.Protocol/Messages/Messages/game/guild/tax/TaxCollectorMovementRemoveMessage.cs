


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TaxCollectorMovementRemoveMessage : Message
{

public const ushort Id = 5915;
public override ushort MessageId
{
    get { return Id; }
}

public int collectorId;
        

public TaxCollectorMovementRemoveMessage()
{
}

public TaxCollectorMovementRemoveMessage(int collectorId)
        {
            this.collectorId = collectorId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(collectorId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

collectorId = reader.ReadInt();
            

}


}


}