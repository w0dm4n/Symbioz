


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayArenaUpdatePlayerInfosMessage : Message
{

public const ushort Id = 6301;
public override ushort MessageId
{
    get { return Id; }
}

public ushort rank;
        public ushort bestDailyRank;
        public ushort bestRank;
        public ushort victoryCount;
        public ushort arenaFightcount;
        

public GameRolePlayArenaUpdatePlayerInfosMessage()
{
}

public GameRolePlayArenaUpdatePlayerInfosMessage(ushort rank, ushort bestDailyRank, ushort bestRank, ushort victoryCount, ushort arenaFightcount)
        {
            this.rank = rank;
            this.bestDailyRank = bestDailyRank;
            this.bestRank = bestRank;
            this.victoryCount = victoryCount;
            this.arenaFightcount = arenaFightcount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(rank);
            writer.WriteVarUhShort(bestDailyRank);
            writer.WriteVarUhShort(bestRank);
            writer.WriteVarUhShort(victoryCount);
            writer.WriteVarUhShort(arenaFightcount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

rank = reader.ReadVarUhShort();
            if ((rank < 0) || (rank > 2300))
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : (rank < 0) || (rank > 2300)");
            bestDailyRank = reader.ReadVarUhShort();
            if ((bestDailyRank < 0) || (bestDailyRank > 2300))
                throw new Exception("Forbidden value on bestDailyRank = " + bestDailyRank + ", it doesn't respect the following condition : (bestDailyRank < 0) || (bestDailyRank > 2300)");
            bestRank = reader.ReadVarUhShort();
            if ((bestRank < 0) || (bestRank > 2300))
                throw new Exception("Forbidden value on bestRank = " + bestRank + ", it doesn't respect the following condition : (bestRank < 0) || (bestRank > 2300)");
            victoryCount = reader.ReadVarUhShort();
            if (victoryCount < 0)
                throw new Exception("Forbidden value on victoryCount = " + victoryCount + ", it doesn't respect the following condition : victoryCount < 0");
            arenaFightcount = reader.ReadVarUhShort();
            if (arenaFightcount < 0)
                throw new Exception("Forbidden value on arenaFightcount = " + arenaFightcount + ", it doesn't respect the following condition : arenaFightcount < 0");
            

}


}


}