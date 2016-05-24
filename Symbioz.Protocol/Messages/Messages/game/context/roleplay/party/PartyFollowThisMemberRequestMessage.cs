


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyFollowThisMemberRequestMessage : PartyFollowMemberRequestMessage
{

public const ushort Id = 5588;
public override ushort MessageId
{
    get { return Id; }
}

public bool enabled;
        

public PartyFollowThisMemberRequestMessage()
{
}

public PartyFollowThisMemberRequestMessage(uint partyId, uint playerId, bool enabled)
         : base(partyId, playerId)
        {
            this.enabled = enabled;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(enabled);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            enabled = reader.ReadBoolean();
            

}


}


}