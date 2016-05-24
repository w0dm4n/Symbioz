


















// Generated on 06/04/2015 18:44:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyMemberRemoveMessage : AbstractPartyEventMessage
{

public const ushort Id = 5579;
public override ushort MessageId
{
    get { return Id; }
}

public uint leavingPlayerId;
        

public PartyMemberRemoveMessage()
{
}

public PartyMemberRemoveMessage(uint partyId, uint leavingPlayerId)
         : base(partyId)
        {
            this.leavingPlayerId = leavingPlayerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(leavingPlayerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            leavingPlayerId = reader.ReadVarUhInt();
            if (leavingPlayerId < 0)
                throw new Exception("Forbidden value on leavingPlayerId = " + leavingPlayerId + ", it doesn't respect the following condition : leavingPlayerId < 0");
            

}


}


}