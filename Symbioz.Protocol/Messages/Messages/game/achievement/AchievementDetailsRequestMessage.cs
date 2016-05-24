


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AchievementDetailsRequestMessage : Message
{

public const ushort Id = 6380;
public override ushort MessageId
{
    get { return Id; }
}

public ushort achievementId;
        

public AchievementDetailsRequestMessage()
{
}

public AchievementDetailsRequestMessage(ushort achievementId)
        {
            this.achievementId = achievementId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(achievementId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

achievementId = reader.ReadVarUhShort();
            if (achievementId < 0)
                throw new Exception("Forbidden value on achievementId = " + achievementId + ", it doesn't respect the following condition : achievementId < 0");
            

}


}


}