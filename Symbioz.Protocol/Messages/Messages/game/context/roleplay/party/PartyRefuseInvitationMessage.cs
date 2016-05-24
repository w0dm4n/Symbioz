


















// Generated on 06/04/2015 18:44:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyRefuseInvitationMessage : AbstractPartyMessage
{

public const ushort Id = 5582;
public override ushort MessageId
{
    get { return Id; }
}



public PartyRefuseInvitationMessage()
{
}

public PartyRefuseInvitationMessage(uint partyId)
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