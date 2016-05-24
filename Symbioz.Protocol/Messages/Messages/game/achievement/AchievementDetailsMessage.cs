


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AchievementDetailsMessage : Message
{

public const ushort Id = 6378;
public override ushort MessageId
{
    get { return Id; }
}

public Types.Achievement achievement;
        

public AchievementDetailsMessage()
{
}

public AchievementDetailsMessage(Types.Achievement achievement)
        {
            this.achievement = achievement;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

achievement.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

achievement = new Types.Achievement();
            achievement.Deserialize(reader);
            

}


}


}