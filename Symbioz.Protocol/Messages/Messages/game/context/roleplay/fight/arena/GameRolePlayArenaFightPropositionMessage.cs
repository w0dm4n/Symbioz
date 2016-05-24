


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayArenaFightPropositionMessage : Message
{

public const ushort Id = 6276;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        public IEnumerable<int> alliesId;
        public ushort duration;
        

public GameRolePlayArenaFightPropositionMessage()
{
}

public GameRolePlayArenaFightPropositionMessage(int fightId, IEnumerable<int> alliesId, ushort duration)
        {
            this.fightId = fightId;
            this.alliesId = alliesId;
            this.duration = duration;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteUShort((ushort)alliesId.Count());
            foreach (var entry in alliesId)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteVarUhShort(duration);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            alliesId = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (alliesId as int[])[i] = reader.ReadInt();
            }
            duration = reader.ReadVarUhShort();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
            

}


}


}