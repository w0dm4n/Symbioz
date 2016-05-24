


















// Generated on 06/04/2015 18:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyCancelInvitationNotificationMessage : AbstractPartyEventMessage
{

public const ushort Id = 6251;
public override ushort MessageId
{
    get { return Id; }
}

public uint cancelerId;
        public uint guestId;
        

public PartyCancelInvitationNotificationMessage()
{
}

public PartyCancelInvitationNotificationMessage(uint partyId, uint cancelerId, uint guestId)
         : base(partyId)
        {
            this.cancelerId = cancelerId;
            this.guestId = guestId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(cancelerId);
            writer.WriteVarUhInt(guestId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            cancelerId = reader.ReadVarUhInt();
            if (cancelerId < 0)
                throw new Exception("Forbidden value on cancelerId = " + cancelerId + ", it doesn't respect the following condition : cancelerId < 0");
            guestId = reader.ReadVarUhInt();
            if (guestId < 0)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0");
            

}


}


}