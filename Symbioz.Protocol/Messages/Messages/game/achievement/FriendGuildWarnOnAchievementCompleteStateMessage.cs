


















// Generated on 06/04/2015 18:44:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class FriendGuildWarnOnAchievementCompleteStateMessage : Message
{

public const ushort Id = 6383;
public override ushort MessageId
{
    get { return Id; }
}

public bool enable;
        

public FriendGuildWarnOnAchievementCompleteStateMessage()
{
}

public FriendGuildWarnOnAchievementCompleteStateMessage(bool enable)
        {
            this.enable = enable;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(enable);
            

}

public override void Deserialize(ICustomDataInput reader)
{

enable = reader.ReadBoolean();
            

}


}


}