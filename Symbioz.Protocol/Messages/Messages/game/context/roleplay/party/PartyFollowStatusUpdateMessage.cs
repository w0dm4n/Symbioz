


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyFollowStatusUpdateMessage : AbstractPartyMessage
{

public const ushort Id = 5581;
public override ushort MessageId
{
    get { return Id; }
}

public bool success;
        public uint followedId;
        

public PartyFollowStatusUpdateMessage()
{
}

public PartyFollowStatusUpdateMessage(uint partyId, bool success, uint followedId)
         : base(partyId)
        {
            this.success = success;
            this.followedId = followedId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(success);
            writer.WriteVarUhInt(followedId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            success = reader.ReadBoolean();
            followedId = reader.ReadVarUhInt();
            if (followedId < 0)
                throw new Exception("Forbidden value on followedId = " + followedId + ", it doesn't respect the following condition : followedId < 0");
            

}


}


}