


















// Generated on 06/04/2015 18:44:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class DungeonPartyFinderRoomContentUpdateMessage : Message
{

public const ushort Id = 6250;
public override ushort MessageId
{
    get { return Id; }
}

public ushort dungeonId;
        public IEnumerable<Types.DungeonPartyFinderPlayer> addedPlayers;
        public IEnumerable<uint> removedPlayersIds;
        

public DungeonPartyFinderRoomContentUpdateMessage()
{
}

public DungeonPartyFinderRoomContentUpdateMessage(ushort dungeonId, IEnumerable<Types.DungeonPartyFinderPlayer> addedPlayers, IEnumerable<uint> removedPlayersIds)
        {
            this.dungeonId = dungeonId;
            this.addedPlayers = addedPlayers;
            this.removedPlayersIds = removedPlayersIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(dungeonId);
            writer.WriteUShort((ushort)addedPlayers.Count());
            foreach (var entry in addedPlayers)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)removedPlayersIds.Count());
            foreach (var entry in removedPlayersIds)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

dungeonId = reader.ReadVarUhShort();
            if (dungeonId < 0)
                throw new Exception("Forbidden value on dungeonId = " + dungeonId + ", it doesn't respect the following condition : dungeonId < 0");
            var limit = reader.ReadUShort();
            addedPlayers = new Types.DungeonPartyFinderPlayer[limit];
            for (int i = 0; i < limit; i++)
            {
                 (addedPlayers as Types.DungeonPartyFinderPlayer[])[i] = new Types.DungeonPartyFinderPlayer();
                 (addedPlayers as Types.DungeonPartyFinderPlayer[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            removedPlayersIds = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (removedPlayersIds as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}