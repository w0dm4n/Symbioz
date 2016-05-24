


















// Generated on 06/04/2015 18:44:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightDispellSpellMessage : GameActionFightDispellMessage
{

public const ushort Id = 6176;
public override ushort MessageId
{
    get { return Id; }
}

public ushort spellId;
        

public GameActionFightDispellSpellMessage()
{
}

public GameActionFightDispellSpellMessage(ushort actionId, int sourceId, int targetId, ushort spellId)
         : base(actionId, sourceId, targetId)
        {
            this.spellId = spellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(spellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            

}


}


}