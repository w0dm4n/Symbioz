


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayPlayerFightFriendlyRequestedMessage : Message
{

public const ushort Id = 5937;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        public uint sourceId;
        public uint targetId;
        

public GameRolePlayPlayerFightFriendlyRequestedMessage()
{
}

public GameRolePlayPlayerFightFriendlyRequestedMessage(int fightId, uint sourceId, uint targetId)
        {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteVarUhInt(sourceId);
            writer.WriteVarUhInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            sourceId = reader.ReadVarUhInt();
            if (sourceId < 0)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < 0");
            targetId = reader.ReadVarUhInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            

}


}


}