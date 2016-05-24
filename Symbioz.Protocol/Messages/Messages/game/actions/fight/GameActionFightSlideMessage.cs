


















// Generated on 06/04/2015 18:44:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightSlideMessage : AbstractGameActionMessage
{

public const ushort Id = 5525;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public short startCellId;
        public short endCellId;
        

public GameActionFightSlideMessage()
{
}

public GameActionFightSlideMessage(ushort actionId, int sourceId, int targetId, short startCellId, short endCellId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.startCellId = startCellId;
            this.endCellId = endCellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(startCellId);
            writer.WriteShort(endCellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            startCellId = reader.ReadShort();
            if ((startCellId < -1) || (startCellId > 559))
                throw new Exception("Forbidden value on startCellId = " + startCellId + ", it doesn't respect the following condition : (startCellId < -1) || (startCellId > 559)");
            endCellId = reader.ReadShort();
            if ((endCellId < -1) || (endCellId > 559))
                throw new Exception("Forbidden value on endCellId = " + endCellId + ", it doesn't respect the following condition : (endCellId < -1) || (endCellId > 559)");
            

}


}


}