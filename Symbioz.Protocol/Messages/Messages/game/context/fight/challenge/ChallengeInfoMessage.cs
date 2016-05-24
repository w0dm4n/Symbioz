


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChallengeInfoMessage : Message
{

public const ushort Id = 6022;
public override ushort MessageId
{
    get { return Id; }
}

public ushort challengeId;
        public int targetId;
        public uint xpBonus;
        public uint dropBonus;
        

public ChallengeInfoMessage()
{
}

public ChallengeInfoMessage(ushort challengeId, int targetId, uint xpBonus, uint dropBonus)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
            this.xpBonus = xpBonus;
            this.dropBonus = dropBonus;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(challengeId);
            writer.WriteInt(targetId);
            writer.WriteVarUhInt(xpBonus);
            writer.WriteVarUhInt(dropBonus);
            

}

public override void Deserialize(ICustomDataInput reader)
{

challengeId = reader.ReadVarUhShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
            xpBonus = reader.ReadVarUhInt();
            if (xpBonus < 0)
                throw new Exception("Forbidden value on xpBonus = " + xpBonus + ", it doesn't respect the following condition : xpBonus < 0");
            dropBonus = reader.ReadVarUhInt();
            if (dropBonus < 0)
                throw new Exception("Forbidden value on dropBonus = " + dropBonus + ", it doesn't respect the following condition : dropBonus < 0");
            

}


}


}