


















// Generated on 06/04/2015 18:44:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyInvitationDungeonDetailsMessage : PartyInvitationDetailsMessage
{

public const ushort Id = 6262;
public override ushort MessageId
{
    get { return Id; }
}

public ushort dungeonId;
        public IEnumerable<bool> playersDungeonReady;
        

public PartyInvitationDungeonDetailsMessage()
{
}

public PartyInvitationDungeonDetailsMessage(uint partyId, sbyte partyType, string partyName, uint fromId, string fromName, uint leaderId, IEnumerable<Types.PartyInvitationMemberInformations> members, IEnumerable<Types.PartyGuestInformations> guests, ushort dungeonId, IEnumerable<bool> playersDungeonReady)
         : base(partyId, partyType, partyName, fromId, fromName, leaderId, members, guests)
        {
            this.dungeonId = dungeonId;
            this.playersDungeonReady = playersDungeonReady;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(dungeonId);
            writer.WriteUShort((ushort)playersDungeonReady.Count());
            foreach (var entry in playersDungeonReady)
            {
                 writer.WriteBoolean(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            dungeonId = reader.ReadVarUhShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            playersDungeonReady = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 (playersDungeonReady as bool[])[i] = reader.ReadBoolean();
            }
            

}


}


}