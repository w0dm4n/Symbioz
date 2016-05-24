


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayShowChallengeMessage : Message
{

public const ushort Id = 301;
public override ushort MessageId
{
    get { return Id; }
}

public Types.FightCommonInformations commonsInfos;
        

public GameRolePlayShowChallengeMessage()
{
}

public GameRolePlayShowChallengeMessage(Types.FightCommonInformations commonsInfos)
        {
            this.commonsInfos = commonsInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

commonsInfos.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

commonsInfos = new Types.FightCommonInformations();
            commonsInfos.Deserialize(reader);
            

}


}


}