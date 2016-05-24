


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildFightPlayersEnemyRemoveMessage : Message
{

public const ushort Id = 5929;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        public uint playerId;
        

public GuildFightPlayersEnemyRemoveMessage()
{
}

public GuildFightPlayersEnemyRemoveMessage(int fightId, uint playerId)
        {
            this.fightId = fightId;
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteVarUhInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            

}


}


}