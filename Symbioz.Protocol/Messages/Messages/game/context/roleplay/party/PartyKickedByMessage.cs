


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyKickedByMessage : AbstractPartyMessage
{

public const ushort Id = 5590;
public override ushort MessageId
{
    get { return Id; }
}

public uint kickerId;
        

public PartyKickedByMessage()
{
}

public PartyKickedByMessage(uint partyId, uint kickerId)
         : base(partyId)
        {
            this.kickerId = kickerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(kickerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            kickerId = reader.ReadVarUhInt();
            if (kickerId < 0)
                throw new Exception("Forbidden value on kickerId = " + kickerId + ", it doesn't respect the following condition : kickerId < 0");
            

}


}


}