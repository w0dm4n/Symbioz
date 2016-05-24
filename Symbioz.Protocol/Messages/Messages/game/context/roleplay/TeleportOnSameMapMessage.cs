


















// Generated on 06/04/2015 18:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TeleportOnSameMapMessage : Message
{

public const ushort Id = 6048;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public ushort cellId;
        

public TeleportOnSameMapMessage()
{
}

public TeleportOnSameMapMessage(int targetId, ushort cellId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(targetId);
            writer.WriteVarUhShort(cellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

targetId = reader.ReadInt();
            cellId = reader.ReadVarUhShort();
            if ((cellId < 0) || (cellId > 559))
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : (cellId < 0) || (cellId > 559)");
            

}


}


}