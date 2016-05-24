


















// Generated on 06/04/2015 18:45:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
{

public const short Id = 207;
public override short TypeId
{
    get { return Id; }
}

public ushort boostedSpellId;
        

public FightTemporarySpellBoostEffect()
{
}

public FightTemporarySpellBoostEffect(uint uid, int targetId, short turnDuration, sbyte dispelable, ushort spellId, uint effectId, uint parentBoostUid, short delta, ushort boostedSpellId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid, delta)
        {
            this.boostedSpellId = boostedSpellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(boostedSpellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            boostedSpellId = reader.ReadVarUhShort();
            if (boostedSpellId < 0)
                throw new Exception("Forbidden value on boostedSpellId = " + boostedSpellId + ", it doesn't respect the following condition : boostedSpellId < 0");
            

}


}


}