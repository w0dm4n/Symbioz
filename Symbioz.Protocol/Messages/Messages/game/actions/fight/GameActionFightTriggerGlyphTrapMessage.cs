


















// Generated on 06/04/2015 18:44:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightTriggerGlyphTrapMessage : AbstractGameActionMessage
{

public const ushort Id = 5741;
public override ushort MessageId
{
    get { return Id; }
}

public short markId;
        public int triggeringCharacterId;
        public ushort triggeredSpellId;
        

public GameActionFightTriggerGlyphTrapMessage()
{
}

public GameActionFightTriggerGlyphTrapMessage(ushort actionId, int sourceId, short markId, int triggeringCharacterId, ushort triggeredSpellId)
         : base(actionId, sourceId)
        {
            this.markId = markId;
            this.triggeringCharacterId = triggeringCharacterId;
            this.triggeredSpellId = triggeredSpellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(markId);
            writer.WriteInt(triggeringCharacterId);
            writer.WriteVarUhShort(triggeredSpellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            markId = reader.ReadShort();
            triggeringCharacterId = reader.ReadInt();
            triggeredSpellId = reader.ReadVarUhShort();
            if (triggeredSpellId < 0)
                throw new Exception("Forbidden value on triggeredSpellId = " + triggeredSpellId + ", it doesn't respect the following condition : triggeredSpellId < 0");
            

}


}


}