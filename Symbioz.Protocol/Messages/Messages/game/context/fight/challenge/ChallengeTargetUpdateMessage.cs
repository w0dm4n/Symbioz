


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChallengeTargetUpdateMessage : Message
{

public const ushort Id = 6123;
public override ushort MessageId
{
    get { return Id; }
}

public ushort challengeId;
        public int targetId;
        

public ChallengeTargetUpdateMessage()
{
}

public ChallengeTargetUpdateMessage(ushort challengeId, int targetId)
        {
            this.challengeId = challengeId;
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(challengeId);
            writer.WriteInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

challengeId = reader.ReadVarUhShort();
            if (challengeId < 0)
                throw new Exception("Forbidden value on challengeId = " + challengeId + ", it doesn't respect the following condition : challengeId < 0");
            targetId = reader.ReadInt();
            

}


}


}