


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyLeaderUpdateMessage : AbstractPartyEventMessage
{

public const ushort Id = 5578;
public override ushort MessageId
{
    get { return Id; }
}

public uint partyLeaderId;
        

public PartyLeaderUpdateMessage()
{
}

public PartyLeaderUpdateMessage(uint partyId, uint partyLeaderId)
         : base(partyId)
        {
            this.partyLeaderId = partyLeaderId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(partyLeaderId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            partyLeaderId = reader.ReadVarUhInt();
            if (partyLeaderId < 0)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0");
            

}


}


}