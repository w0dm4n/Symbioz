


















// Generated on 06/04/2015 18:44:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightChangeLookMessage : AbstractGameActionMessage
{

public const ushort Id = 5532;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public Types.EntityLook entityLook;
        

public GameActionFightChangeLookMessage()
{
}

public GameActionFightChangeLookMessage(ushort actionId, int sourceId, int targetId, Types.EntityLook entityLook)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.entityLook = entityLook;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            entityLook.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            entityLook = new Types.EntityLook();
            entityLook.Deserialize(reader);
            

}


}


}