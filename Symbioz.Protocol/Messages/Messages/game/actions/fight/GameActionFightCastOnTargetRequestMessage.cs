


















// Generated on 06/04/2015 18:44:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightCastOnTargetRequestMessage : Message
{

public const ushort Id = 6330;
public override ushort MessageId
{
    get { return Id; }
}

public ushort spellId;
        public int targetId;
        

public GameActionFightCastOnTargetRequestMessage()
{
}

public GameActionFightCastOnTargetRequestMessage(ushort spellId, int targetId)
        {
            this.spellId = spellId;
            this.targetId = targetId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(spellId);
            writer.WriteInt(targetId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            targetId = reader.ReadInt();
            

}


}


}