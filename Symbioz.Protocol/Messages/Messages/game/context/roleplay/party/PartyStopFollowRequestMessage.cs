


















// Generated on 06/04/2015 18:44:38
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyStopFollowRequestMessage : AbstractPartyMessage
{

public const ushort Id = 5574;
public override ushort MessageId
{
    get { return Id; }
}



public PartyStopFollowRequestMessage()
{
}

public PartyStopFollowRequestMessage(uint partyId)
         : base(partyId)
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