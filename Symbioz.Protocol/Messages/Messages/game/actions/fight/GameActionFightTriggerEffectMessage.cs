


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightTriggerEffectMessage : GameActionFightDispellEffectMessage
{

public const ushort Id = 6147;
public override ushort MessageId
{
    get { return Id; }
}



public GameActionFightTriggerEffectMessage()
{
}

public GameActionFightTriggerEffectMessage(ushort actionId, int sourceId, int targetId, int boostUID)
         : base(actionId, sourceId, targetId, boostUID)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}