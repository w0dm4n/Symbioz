


















// Generated on 06/04/2015 18:45:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class CharacterSpellModification
{

public const short Id = 215;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte modificationType;
        public ushort spellId;
        public Types.CharacterBaseCharacteristic value;
        

public CharacterSpellModification()
{
}

public CharacterSpellModification(sbyte modificationType, ushort spellId, Types.CharacterBaseCharacteristic value)
        {
            this.modificationType = modificationType;
            this.spellId = spellId;
            this.value = value;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(modificationType);
            writer.WriteVarUhShort(spellId);
            value.Serialize(writer);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

modificationType = reader.ReadSByte();
            if (modificationType < 0)
                throw new Exception("Forbidden value on modificationType = " + modificationType + ", it doesn't respect the following condition : modificationType < 0");
            spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            value = new Types.CharacterBaseCharacteristic();
            value.Deserialize(reader);
            

}


}


}