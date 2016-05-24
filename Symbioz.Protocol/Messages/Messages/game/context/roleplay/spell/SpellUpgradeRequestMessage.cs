


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SpellUpgradeRequestMessage : Message
{

public const ushort Id = 5608;
public override ushort MessageId
{
    get { return Id; }
}

public ushort spellId;
        public sbyte spellLevel;
        

public SpellUpgradeRequestMessage()
{
}

public SpellUpgradeRequestMessage(ushort spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(spellId);
            writer.WriteSByte(spellLevel);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if ((spellLevel < 1) || (spellLevel > 6))
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : (spellLevel < 1) || (spellLevel > 6)");
            

}


}


}