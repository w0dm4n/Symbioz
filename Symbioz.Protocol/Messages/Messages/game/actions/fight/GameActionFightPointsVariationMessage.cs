


















// Generated on 06/04/2015 18:44:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightPointsVariationMessage : AbstractGameActionMessage
{

public const ushort Id = 1030;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public short delta;
        

public GameActionFightPointsVariationMessage()
{
}

public GameActionFightPointsVariationMessage(ushort actionId, int sourceId, int targetId, short delta)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(delta);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadShort();
            

}


}


}