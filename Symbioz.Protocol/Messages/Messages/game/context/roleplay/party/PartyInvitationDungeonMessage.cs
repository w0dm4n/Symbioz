


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationDungeonMessage : PartyInvitationMessage
{

public const ushort Id = 6244;
public override ushort MessageId
{
    get { return Id; }
}

public ushort dungeonId;
        

public PartyInvitationDungeonMessage()
{
}

public PartyInvitationDungeonMessage(uint partyId, sbyte partyType, string partyName, sbyte maxParticipants, uint fromId, string fromName, uint toId, ushort dungeonId)
         : base(partyId, partyType, partyName, maxParticipants, fromId, fromName, toId)
        {
            this.dungeonId = dungeonId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(dungeonId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            dungeonId = reader.ReadVarUhShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            

}


}


}