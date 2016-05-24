


















// Generated on 06/04/2015 18:45:33
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class SpellItem : Item
{
    public bool needToBeSaved = false;
public const short Id = 49;
public override short TypeId
{
    get { return Id; }
}
public int characterId;
public byte position;
        public int spellId;
        public sbyte spellLevel;
        

public SpellItem()
{
}

public SpellItem(byte position, int spellId, sbyte spellLevel)
        {
            this.position = position;
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(position);
            writer.WriteInt(spellId);
            writer.WriteSByte(spellLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            position = reader.ReadByte();
            if ((position < 63) || (position > 255))
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : (position < 63) || (position > 255)");
            spellId = reader.ReadInt();
            spellLevel = reader.ReadSByte();
            if ((spellLevel < 1) || (spellLevel > 6))
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : (spellLevel < 1) || (spellLevel > 6)");
            

}


}


}