


















// Generated on 06/04/2015 18:44:28
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayPlayerFightRequestMessage : Message
{

public const ushort Id = 5731;
public override ushort MessageId
{
    get { return Id; }
}

public uint targetId;
        public short targetCellId;
        public bool friendly;
        

public GameRolePlayPlayerFightRequestMessage()
{
}

public GameRolePlayPlayerFightRequestMessage(uint targetId, short targetCellId, bool friendly)
        {
            this.targetId = targetId;
            this.targetCellId = targetCellId;
            this.friendly = friendly;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(targetId);
            writer.WriteShort(targetCellId);
            writer.WriteBoolean(friendly);
            

}

public override void Deserialize(ICustomDataInput reader)
{

targetId = reader.ReadVarUhInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            targetCellId = reader.ReadShort();
            if ((targetCellId < -1) || (targetCellId > 559))
                throw new Exception("Forbidden value on targetCellId = " + targetCellId + ", it doesn't respect the following condition : (targetCellId < -1) || (targetCellId > 559)");
            friendly = reader.ReadBoolean();
            

}


}


}