


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightThrowCharacterMessage : AbstractGameActionMessage
{

public const ushort Id = 5829;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public short cellId;
        

public GameActionFightThrowCharacterMessage()
{
}

public GameActionFightThrowCharacterMessage(ushort actionId, int sourceId, int targetId, short cellId)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(cellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            cellId = reader.ReadShort();
            if ((cellId < -1) || (cellId > 559))
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : (cellId < -1) || (cellId > 559)");
            

}


}


}