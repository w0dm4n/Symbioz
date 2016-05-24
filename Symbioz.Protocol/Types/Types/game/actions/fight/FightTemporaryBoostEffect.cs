


















// Generated on 06/04/2015 18:45:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightTemporaryBoostEffect : AbstractFightDispellableEffect
{

public const short Id = 209;
public override short TypeId
{
    get { return Id; }
}

public short delta;
        

public FightTemporaryBoostEffect()
{
}

public FightTemporaryBoostEffect(uint uid, int targetId, short turnDuration, sbyte dispelable, ushort spellId, uint effectId, uint parentBoostUid, short delta)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid)
        {
            this.delta = delta;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(delta);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            delta = reader.ReadShort();
            

}


}


}