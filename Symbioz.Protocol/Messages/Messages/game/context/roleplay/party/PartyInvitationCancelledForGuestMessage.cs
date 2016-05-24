


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationCancelledForGuestMessage : AbstractPartyMessage
{

public const ushort Id = 6256;
public override ushort MessageId
{
    get { return Id; }
}

public uint cancelerId;
        

public PartyInvitationCancelledForGuestMessage()
{
}

public PartyInvitationCancelledForGuestMessage(uint partyId, uint cancelerId)
         : base(partyId)
        {
            this.cancelerId = cancelerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(cancelerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            cancelerId = reader.ReadVarUhInt();
            if (cancelerId < 0)
                throw new Exception("Forbidden value on cancelerId = " + cancelerId + ", it doesn't respect the following condition : cancelerId < 0");
            

}


}


}