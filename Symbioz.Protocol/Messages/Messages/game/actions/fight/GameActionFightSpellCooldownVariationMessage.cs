


















// Generated on 06/04/2015 18:44:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightSpellCooldownVariationMessage : AbstractGameActionMessage
{

public const ushort Id = 6219;
public override ushort MessageId
{
    get { return Id; }
}

public int targetId;
        public ushort spellId;
        public short value;
        

public GameActionFightSpellCooldownVariationMessage()
{
}

public GameActionFightSpellCooldownVariationMessage(ushort actionId, int sourceId, int targetId, ushort spellId, short value)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.spellId = spellId;
            this.value = value;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteVarUhShort(spellId);
            writer.WriteVarShort(value);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = reader.ReadVarShort();
            

}


}


}