


















// Generated on 06/04/2015 18:44:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightDispellableEffectMessage : AbstractGameActionMessage
{

public const ushort Id = 6070;
public override ushort MessageId
{
    get { return Id; }
}

public Types.AbstractFightDispellableEffect effect;
        

public GameActionFightDispellableEffectMessage()
{
}

public GameActionFightDispellableEffectMessage(ushort actionId, int sourceId, Types.AbstractFightDispellableEffect effect)
         : base(actionId, sourceId)
        {
            this.effect = effect;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(effect.TypeId);
            effect.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            effect = Types.ProtocolTypeManager.GetInstance<Types.AbstractFightDispellableEffect>(reader.ReadShort());
            effect.Deserialize(reader);
            

}


}


}