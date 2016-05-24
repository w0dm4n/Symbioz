


















// Generated on 06/04/2015 18:45:20
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightTemporaryBoostStateEffect : FightTemporaryBoostEffect
{

public const short Id = 214;
public override short TypeId
{
    get { return Id; }
}

public short stateId;
        

public FightTemporaryBoostStateEffect()
{
}

public FightTemporaryBoostStateEffect(uint uid, int targetId, short turnDuration, sbyte dispelable, ushort spellId, uint effectId, uint parentBoostUid, short delta, short stateId)
         : base(uid, targetId, turnDuration, dispelable, spellId, effectId, parentBoostUid, delta)
        {
            this.stateId = stateId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteShort(stateId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            stateId = reader.ReadShort();
            

}


}


}