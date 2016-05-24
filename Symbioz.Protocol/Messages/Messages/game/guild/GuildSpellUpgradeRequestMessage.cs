


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildSpellUpgradeRequestMessage : Message
{

public const ushort Id = 5699;
public override ushort MessageId
{
    get { return Id; }
}

public int spellId;
        

public GuildSpellUpgradeRequestMessage()
{
}

public GuildSpellUpgradeRequestMessage(int spellId)
        {
            this.spellId = spellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(spellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellId = reader.ReadInt();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            

}


}


}