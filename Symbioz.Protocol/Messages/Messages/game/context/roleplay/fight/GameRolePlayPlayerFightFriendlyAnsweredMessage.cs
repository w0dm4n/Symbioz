


















// Generated on 06/04/2015 18:44:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayPlayerFightFriendlyAnsweredMessage : Message
{

public const ushort Id = 5733;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        public uint sourceId;
        public uint targetId;
        public bool accept;
        

public GameRolePlayPlayerFightFriendlyAnsweredMessage()
{
}

public GameRolePlayPlayerFightFriendlyAnsweredMessage(int fightId, uint sourceId, uint targetId, bool accept)
        {
            this.fightId = fightId;
            this.sourceId = sourceId;
            this.targetId = targetId;
            this.accept = accept;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteVarUhInt(sourceId);
            writer.WriteVarUhInt(targetId);
            writer.WriteBoolean(accept);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            sourceId = reader.ReadVarUhInt();
            if (sourceId < 0)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < 0");
            targetId = reader.ReadVarUhInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            accept = reader.ReadBoolean();
            

}


}


}