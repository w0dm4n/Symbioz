


















// Generated on 06/04/2015 18:44:57
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkMulticraftCrafterMessage : Message
{

public const ushort Id = 5818;
public override ushort MessageId
{
    get { return Id; }
}

public uint skillId;
        

public ExchangeStartOkMulticraftCrafterMessage()
{
}

public ExchangeStartOkMulticraftCrafterMessage(uint skillId)
        {
            this.skillId = skillId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(skillId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

skillId = reader.ReadVarUhInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            

}


}


}