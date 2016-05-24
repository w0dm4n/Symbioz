


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SpellUpgradeSuccessMessage : Message
{

public const ushort Id = 1201;
public override ushort MessageId
{
    get { return Id; }
}

public int spellId;
        public sbyte spellLevel;
        

public SpellUpgradeSuccessMessage()
{
}

public SpellUpgradeSuccessMessage(int spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(spellId);
            writer.WriteSByte(spellLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellId = reader.ReadInt();
            spellLevel = reader.ReadSByte();
            if ((spellLevel < 1) || (spellLevel > 6))
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : (spellLevel < 1) || (spellLevel > 6)");
            

}


}


}