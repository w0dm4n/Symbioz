


















// Generated on 06/04/2015 18:45:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class AbstractFightDispellableEffect
{

public const short Id = 206;
public virtual short TypeId
{
    get { return Id; }
}

public uint uid;
        public int targetId;
        public short turnDuration;
        public sbyte dispelable;
        public ushort spellId;
        public uint effectId;
        public uint parentBoostUid;
        

public AbstractFightDispellableEffect()
{
}

public AbstractFightDispellableEffect(uint uid, int targetId, short turnDuration, sbyte dispelable, ushort spellId, uint effectId, uint parentBoostUid)
        {
            this.uid = uid;
            this.targetId = targetId;
            this.turnDuration = turnDuration;
            this.dispelable = dispelable;
            this.spellId = spellId;
            this.effectId = effectId;
            this.parentBoostUid = parentBoostUid;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(uid);
            writer.WriteInt(targetId);
            writer.WriteShort(turnDuration);
            writer.WriteSByte(dispelable);
            writer.WriteVarUhShort(spellId);
            writer.WriteVarUhInt(effectId);
            writer.WriteVarUhInt(parentBoostUid);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

uid = reader.ReadVarUhInt();
            if (uid < 0)
                throw new Exception("Forbidden value on uid = " + uid + ", it doesn't respect the following condition : uid < 0");
            targetId = reader.ReadInt();
            turnDuration = reader.ReadShort();
            dispelable = reader.ReadSByte();
            if (dispelable < 0)
                throw new Exception("Forbidden value on dispelable = " + dispelable + ", it doesn't respect the following condition : dispelable < 0");
            spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            effectId = reader.ReadVarUhInt();
            if (effectId < 0)
                throw new Exception("Forbidden value on effectId = " + effectId + ", it doesn't respect the following condition : effectId < 0");
            parentBoostUid = reader.ReadVarUhInt();
            if (parentBoostUid < 0)
                throw new Exception("Forbidden value on parentBoostUid = " + parentBoostUid + ", it doesn't respect the following condition : parentBoostUid < 0");
            

}


}


}