


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ValidateSpellForgetMessage : Message
{

public const ushort Id = 1700;
public override ushort MessageId
{
    get { return Id; }
}

public ushort spellId;
        

public ValidateSpellForgetMessage()
{
}

public ValidateSpellForgetMessage(ushort spellId)
        {
            this.spellId = spellId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(spellId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            

}


}


}