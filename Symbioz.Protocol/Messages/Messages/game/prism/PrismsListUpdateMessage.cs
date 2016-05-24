


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismsListUpdateMessage : PrismsListMessage
{

public const ushort Id = 6438;
public override ushort MessageId
{
    get { return Id; }
}



public PrismsListUpdateMessage()
{
}

public PrismsListUpdateMessage(IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
         : base(prisms)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}